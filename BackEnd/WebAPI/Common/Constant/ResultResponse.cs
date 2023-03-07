using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeManagement.Common.Constant.Enum;

namespace EmployeeManagement.Common.Constant
{
    public class ResultData<DataType>
    {
        [SwaggerSchema("Dữ liệu trả về client")]
        public DataType Data { get; set; }

        [SwaggerSchema("Mã code trình duyệt")]
        public int StatusCode { get; set; }

        [SwaggerSchema("Thông báo lỗi nội bộ trên server")]
        public string InternalMessage { get; set; }

    }

    public class ApiResponse : ObjectResult
    {
        private ResultData<object> ResultData { get; set; } = new();

        /// <summary>
        /// Đối tượng chứa dữ liệu chính trả về cho client
        /// </summary>
        public object Data { get => ResultData.Data; set => ResultData.Data = value; }

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
        /// <summary>
        /// Message để ghi chú, lưu trữ trên server
        /// </summary>
        public string InternalMessage { get => ResultData.InternalMessage; set => ResultData.InternalMessage = value; }

        public ApiResponse(int statusCode, object data =null, string internalMessage = null) : base(null)
        {
            ResultData = new();
            StatusCode = statusCode;
            Data = data;
            Value = ResultData;
            InternalMessage = internalMessage;
        }
    }

    public class ApiOkResultResponse : ApiResponse
    {
        public ApiOkResultResponse(object data = null) : base(StatusCodes.Status200OK, data)
        {

        }
    }

    public class ApiInvalidParamResponse : ApiResponse
    {
        public ApiInvalidParamResponse(string userMessage = null, string internalmessage = null) : base(StatusCodes.Status400BadRequest,null, userMessage)
        {

        }
    }


    public class ResultResponse
    {

        public dynamic Result { get; set; }

        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public int TotalItems { get; set; }
        public string Messenger { get; set; }

        public ResultResponse(int statusCode, object data = null, string messageCode = null) 
        {
            StatusCode = statusCode;
            Result = data;    
        }
    }

   


}
