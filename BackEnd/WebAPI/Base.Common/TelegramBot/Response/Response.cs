using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.TelegramBot
{
    public sealed class Response<TResult>
    {
        public TResult Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public bool Ok { get; set; }


        public Response(TResult result, HttpStatusCode status, string message, bool ok)
        {
            Data = result;
            StatusCode = status;
            Message = message;
            Ok = ok;
        }
    }
}
