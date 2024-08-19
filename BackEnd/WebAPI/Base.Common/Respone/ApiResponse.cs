using Base.Common.Constant;
using Base.Common.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Base.Common.Respone
{
    internal class ResultData<T>
    {
        public int StatusCode { get; set; }

        public bool Success => StatusCode == StatusCodes.Status200OK || StatusCode == StatusCodes.Status204NoContent;

        public ResponseCodeEnums ResponseCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserMessage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InternalMessage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MessageCode { get; set; }
    }

    public class ApiResponse<T> : ObjectResult
    {
        [JsonIgnore]
        private ResultData<T> ResultData { get; set; } = new();

        /// <summary> Mã code trình duyệt (Lưu ý: riêng trường hợp gán = 204 thì code trình duyệt vẫn = 200) </summary>
        public new int StatusCode
        {
            get => ResultData.StatusCode;
            set
            {
                // Trường hợp 204 trình duyệt không hỗ trợ content nên phải gán lại
                base.StatusCode = value == StatusCodes.Status204NoContent ? StatusCodes.Status200OK : value;
                ResultData.StatusCode = value;
            }
        }

        /// <summary> Trạng thái khi thực thi thành công (200 hoặc 204) </summary>
        public bool Success => ResultData.Success;

        /// <summary> Đối tượng chứa dữ liệu chính trả về cho client </summary>
        public T Data { get => ResultData.Data; set => ResultData.Data = value; }

        /// <summary> Message trả về cho client, thường là LanguageCode </summary>
        public string UserMessage { get => ResultData.UserMessage; set => ResultData.UserMessage = value; }

        /// <summary> Message để ghi chú, lưu trữ trên server </summary>
        public string InternalMessage { get => ResultData.InternalMessage; set => ResultData.InternalMessage = value; }

        public ResponseCodeEnums ResponseCode { get => ResultData.ResponseCode; set => ResultData.ResponseCode = value; }

        public string MessageCode { get => ResultData.MessageCode; set => ResultData.MessageCode = value; }

        public ApiResponse(int statusCode, T data = default, string userMessage = null, string internalMessage = null, ResponseCodeEnums responseCode = ResponseCodeEnums.Success, string messageCode = null) : base(null)
        {
            ResultData = new();
            StatusCode = statusCode;
            Data = data;
            ResponseCode = responseCode;
            UserMessage = userMessage ?? GetDefaultUserMessageForStatusCode(statusCode);
            InternalMessage = internalMessage ?? GetDefaultInternalMessageForStatusCode(statusCode);
            MessageCode = messageCode;
            Value = ResultData;
        }

        private static string GetDefaultUserMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status404NotFound => "Có lỗi xảy ra bạn vui lòng kiểm tra lại",
                StatusCodes.Status500InternalServerError => "Có lỗi xảy ra bạn vui lòng kiểm tra lại",
                _ => null,
            };
        }

        private static string GetDefaultInternalMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status404NotFound => "Resource not found",
                StatusCodes.Status500InternalServerError => "An unhandled error occurred",
                _ => null,
            };
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse(int statusCode, object data = null, string userMessage = null, string internalMessage = null, ResponseCodeEnums responseCode = ResponseCodeEnums.Success, string messageCode = null) : base(statusCode, data, userMessage, internalMessage, responseCode, messageCode)
        {
        }
    }

    public class ApiForbidResponse : ApiResponse
    {
        public ApiForbidResponse(string userMessage = null, string internalMessage = null) : base(StatusCodes.Status403Forbidden, null,null, internalMessage ?? "Không có quyền", ResponseCodeEnums.UnAuthorize)
        {
        }
    }
}
