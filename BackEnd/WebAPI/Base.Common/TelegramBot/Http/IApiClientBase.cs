using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.TelegramBot.Http
{
    public interface IApiClientBase
    {
        /// <summary>Lấy tuần tự</summary>
        Response<T> Get<T>(string requestUrl);

        /// <summary>Lấy Async</summary>
        Task<Response<T>> GetAsync<T>(string requestUrl);

        /// <summary></summary>
        Response<T> Post<T>(string requestUrl, object body, Dictionary<string, string> headerParams = null);
    }
}
