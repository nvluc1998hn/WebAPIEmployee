using Base.Common.Swagger;
using FluentValidation.Results;

namespace Base.Common.Validation
{
    public class BaseValidatorMessage
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

    }
}
