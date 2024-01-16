using Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Base.Common.TelegramBot;
using Serilog;

namespace Base.Common.TelegramBot.Http
{
    public class ApiClientBase: IApiClientBase
    {
        /// <summary>
        /// Tên đăng nhập vào API
        /// Nếu không cần thì để trống
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  24/06/2022   created
        /// </Modified>
        protected virtual string Username => string.Empty;

        /// <summary>
        /// Mật khẩu vào API
        /// Nếu không cần thì để trống
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  24/06/2022   created
        /// </Modified>
        protected virtual string Password => string.Empty;

        protected string AuthorizationKey
        {
            get
            {
                string key = string.Empty;

                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                {
                    key = Base64Helper.Base64Encode($"{Username}:{Password}");
                }
                return key;
            }
        }

        protected virtual Response<T> DoRequest<T>(string endpoint, HttpMethod method, object body = null, Dictionary<string, string> headerParams = null)
        {
            Response<T> response = null;
            string responseBody = string.Empty;
            HttpRequestMessage httpRequest = null;
            HttpResponseMessage httpResponse = null;
            try
            {
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                using HttpClient client = new HttpClient(handler);

                // Nếu dùng Basic Authentication
                if (!string.IsNullOrEmpty(AuthorizationKey))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthorizationKey);
                }

                string json = string.Empty;

                if (body != null)
                {
                    json = Serialize(body);
                }

                httpRequest = new HttpRequestMessage(method, endpoint);

                string contentType = string.Empty;

                if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    contentType = "application/json";
                }
                else
                {
                    contentType = "application/json; charset=utf-8";
                }

                if (!string.IsNullOrEmpty(json))
                {
                    httpRequest.Content = new StringContent(json, Encoding.UTF8, contentType);
                }

                if (headerParams != null && headerParams.Count > 0)
                {
                    foreach (var item in headerParams)
                    {
                        httpRequest.Headers.Add(item.Key, item.Value);
                    }
                }

                httpResponse = client.Send(httpRequest);

                using StreamReader reader = new StreamReader(httpResponse.Content.ReadAsStream());
                responseBody = reader.ReadToEnd();

                if (httpResponse.IsSuccessStatusCode)
                {

                    response = new Response<T>(JsonConvert.DeserializeObject<T>(responseBody), httpResponse.StatusCode, String.Empty, true);
                }
                else
                {
                    response = new Response<T>(JsonConvert.DeserializeObject<T>(responseBody), httpResponse.StatusCode, "Có lỗi trong quá trình gửi, vui lòng kiểm tra lại log.", false);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Fatal("{0} has an exception: {1}", MethodInfo.GetCurrentMethod().Name, ex);
            }

            finally
            {
                httpRequest?.Dispose();

                httpResponse?.Dispose();
            }

            return response;
        }

        protected virtual async Task<Response<T>> DoRequestAsync<T>(string endpoint, HttpMethod method, object body = null, Dictionary<string, string> headerParams = null)
        {
            Response<T> response = null;
            string responseBody = string.Empty;
            HttpRequestMessage httpRequest = null;
            HttpResponseMessage httpResponse = null;
            try
            {
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                using HttpClient client = new HttpClient(handler);

                // Nếu dùng Basic Authentication
                if (!string.IsNullOrEmpty(AuthorizationKey))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthorizationKey);
                }

                string json = string.Empty;

                if (body != null)
                {
                    json = Serialize(body);
                }

                httpRequest = new HttpRequestMessage(method, endpoint);

                string contentType = string.Empty;

                if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    contentType = "application/json";
                }
                else
                {
                    contentType = "application/json; charset=utf-8";
                }

                if (!string.IsNullOrEmpty(json))
                {
                    httpRequest.Content = new StringContent(json, Encoding.UTF8, contentType);
                }

                if (headerParams != null && headerParams.Count > 0)
                {
                    foreach (var item in headerParams)
                    {
                        httpRequest.Headers.Add(item.Key, item.Value);
                    }
                }

                httpResponse = await client.SendAsync(httpRequest);

                using StreamReader reader = new StreamReader(httpResponse.Content.ReadAsStream());
                responseBody = await reader.ReadToEndAsync();

                if (httpResponse.IsSuccessStatusCode)
                {

                    response = new Response<T>(JsonConvert.DeserializeObject<T>(responseBody), httpResponse.StatusCode, String.Empty, true);
                }
                else
                {
                    response = new Response<T>(JsonConvert.DeserializeObject<T>(responseBody), httpResponse.StatusCode, "Có lỗi trong quá trình gửi, vui lòng kiểm tra lại log.", false);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Fatal("{0} has an exception: {1}", MethodInfo.GetCurrentMethod().Name, ex);
            }

            finally
            {
                httpRequest?.Dispose();

                httpResponse?.Dispose();
            }

            return response;
        }

        protected string Serialize<T>(T data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return JsonConvert.SerializeObject(data, settings);
        }

        /// <summary>
        /// Method: Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl">Đường dẫn của API</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  24/06/2022   created
        /// </Modified>
        public Response<T> Get<T>(string requestUrl)
        {
            return DoRequest<T>(requestUrl, HttpMethod.Get);
        }

        /// <summary>
        /// Method: Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl">Đường dẫn của API</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  24/06/2022   created
        /// </Modified>
        public async Task<Response<T>> GetAsync<T>(string requestUrl)
        {
            return await DoRequestAsync<T>(requestUrl, HttpMethod.Get);
        }

        /// <summary>
        /// Method: Post
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headerParams">The header parameters.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  24/06/2022   created
        /// </Modified>
        public Response<T> Post<T>(string requestUrl, object body, Dictionary<string, string> headerParams = null)
        {
            return DoRequest<T>(requestUrl, HttpMethod.Post, body, headerParams);
        }

        /// <summary>
        /// Method: Post
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headerParams">The header parameters.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  24/06/2022   created
        /// </Modified>
        public async Task<Response<T>> PostAsync<T>(string requestUrl, object body, Dictionary<string, string> headerParams = null)
        {
            return await DoRequestAsync<T>(requestUrl, HttpMethod.Post, body, headerParams);
        }
    }
}
