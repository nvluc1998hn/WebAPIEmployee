using Base.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Helper
{
    /// <summary>
    /// Định nghĩa các method làm việc với RESTAPI
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// </Modified>
    public interface IServiceCommunication
    {
        /// <summary> GET call chéo v3 </summary>
        Task<T> GetAsync<T>(string clientSource, string url, object[] otherParams = null, bool withToken = true, int? timeout = null);

        Task<T> GetDataAsync<T>(string clientSource, string url, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer, int? timeout = null);

        /// <summary> POST call chéo v3 </summary>
        Task<T> PostAsync<T>(string clientSource, string url, object body, string token, TokenTypeEnum tokenType, int timeOut = (int)HttpClientTimeOutEnum.TimeOut);

        /// <summary> POST call chéo v3 </summary>
        Task<T> PostAsync<T>(string clientSource, string url, object body, bool withToken = true, int timeOut = (int)HttpClientTimeOutEnum.TimeOut);

        Task<T> PostDataAsync<T>(string clientSource, string url, object data, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer, int timeOut = (int)HttpClientTimeOutEnum.TimeOut);

        Task<T> PostAsyncNew<T>(string clientSource, string url, object body, bool withToken = true);

        /// <summary> PUT call chéo v3 </summary>
        Task<T> PutAsync<T>(string clientSource, string url, object body, bool withToken = true);

        Task<T> PutDataAsync<T>(string clientSource, string url, object data, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer);

        Task<T> DeleteDataAsync<T>(string clientSource, string url, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer);

        /// <summary> POST trả về dữ liệu và status code </summary>
        Task<PostStatusData<T>> PostStatusAsync<T>(string clientSource, string url, object body, bool withToken = true, int timeOut = (int)HttpClientTimeOutEnum.TimeOut);

        /// <summary> GET trả về dữ liệu và status code </summary>
        Task<PostStatusData<T>> GetStatusAsync<T>(string clientSource, string url, bool withToken = true);
    }

    public class PostStatusData<TData>
    {
        public TData Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get => StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.NoContent; }
    }
}
