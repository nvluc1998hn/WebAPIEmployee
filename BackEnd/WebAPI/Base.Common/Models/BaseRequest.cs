using Base.Common.Swagger;
using FluentValidation.Results;

namespace Base.Common.Models
{
    public class BaseRequest
    {
        public virtual bool IsValid()
        {
            return true;
        }

        /// <summary> API trả về kết quả validate và message tương ứng nếu có. Client không cần truyền </summary>
        [SwaggerHide]
        public virtual ValidationResult ValidationResult { get; set; }

        [SwaggerHide]
        public virtual List<string> ValidationResultCustomMessages { get; set; }

        [SwaggerHide]
        public virtual string ValidationResultMessage
        {
            get
            {
                List<string> messages = new();

                if (ValidationResult?.Errors?.Count > 0)
                {
                    messages.AddRange(ValidationResult.Errors.Select(x => x.ErrorMessage));
                }

                if (ValidationResultCustomMessages?.Count > 0)
                {
                    messages.AddRange(ValidationResultCustomMessages);
                }

                var message = messages.Any() ? string.Join(",", messages.Distinct()) : null;
                return message;
            }
        }

        /// <summary> Kiểm tra trạng thái validate theo độ ưu tiên mà không cần gọi lại hàm Validate nhiều lần </summary>
        [SwaggerHide]
        public virtual bool Valid
        {
            get
            {
                bool ok;

                if (ValidationResult != null)
                {
                    ok = ValidationResult.IsValid;
                }
                else
                {
                    ok = ValidationResultCustomMessages?.Any() != true && IsValid();
                }

                return ok;
            }
        }
    }
}
