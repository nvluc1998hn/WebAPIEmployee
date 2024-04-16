using Base.Common.Enum;
using Base.Common.Jwt;
using Base.Common.Respone;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Helper
{
    public class ServiceCommunication : IServiceCommunication
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<ServiceCommunication> _logger;

        /// <summary> Số giây timeout </summary>
        private const int MILISECOND_TIMEOUT = 5000;

        public ServiceCommunication(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<ServiceCommunication> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BA JWT Service");
        }

        public async Task<T> DeleteDataAsync<T>(string clientSource, string url, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer)
        {
            if (string.IsNullOrEmpty(url)) return default;
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (withToken)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        token = _httpContextAccessor.HttpContext.GetRequestToken();
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Remove("Authorization");
                        switch (tokenType)
                        {
                            case TokenTypeEnum.Bearer:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;

                            case TokenTypeEnum.Basic:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
                                break;

                            default:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;
                        }
                    }
                }
                var cts = new CancellationTokenSource();
                cts.CancelAfter(MILISECOND_TIMEOUT);

                // Call Post URL
                var delResult = await _httpClient.DeleteAsync(url);

                return await ConvertToDataObject<T>(delResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi DeleteDataAsync clientSource: {clientSource}, url {url}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
             //   TelegramDeployHelper.SentError($"ServiceCommunication.DeleteDataAsync() clientSource:{clientSource}", ex, "url: {0}", new object[] { url });

                return default;
            }
        }

        public async Task<T> GetAsync<T>(string clientSource, string url, object[] otherParams = null, bool withToken = true, int? timeout = null)
        {
            T data = default;

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    if (otherParams?.Length > 0)
                    {
                        url += "/" + string.Join("/", otherParams);
                    }

                    var res = await GetDataAsync<ApiResponse<T>>(clientSource, url, withToken, "", TokenTypeEnum.Bearer, timeout);
                    if (res != null)
                    {
                        data = res.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi GetAsync clientSource: {clientSource}, url {url}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
                //TelegramDeployHelper.SentError($"ServiceCommunication.GetAsync() clientSource: {clientSource}", ex, "url: {0}", new object[] { url });
            }

            return data;
        }

        public async Task<T> GetDataAsync<T>(string clientSource, string url, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer, int? timeout = null)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (withToken)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        token = _httpContextAccessor.HttpContext.GetRequestToken();
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Remove("Authorization");
                        switch (tokenType)
                        {
                            case TokenTypeEnum.Bearer:
                                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;

                            case TokenTypeEnum.Basic:
                                client.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
                                break;

                            default:
                                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;
                        }
                    }
                }

                if (timeout > 0)
                {
                    client.Timeout = TimeSpan.FromMilliseconds(timeout.Value);
                }

                var blResult = await client.GetStringAsync(url);
                T blResponse = JsonConvert.DeserializeObject<T>(blResult);
                return blResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{clientSource} Lỗi GetDataAsync với url {url}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
               // TelegramDeployHelper.SentError($"{clientSource} gọi ServiceCommunication.GetDataAsync() 2", ex, "url: {0}, token: {1}", new object[] { url, token });

                return default;
            }
        }

        public async Task<PostStatusData<T>> GetStatusAsync<T>(string clientSource, string url, bool withToken = true)
        {
            var data = new PostStatusData<T>() { Data = default };

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var res = await GetDataAsync<ApiResponse<T>>(clientSource, url, withToken);
                    if (res != null)
                    {
                        data.StatusCode = (HttpStatusCode)res.StatusCode;
                        data.Data = res.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi PostStatusAsync, clientSource: {clientSource}, url {url}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
               // TelegramDeployHelper.SentError($"ServiceCommunication.PostStatusAsync() clientSource: {clientSource}", ex, "url: {0}, body: {1}", new object[] { url });
            }

            return data;
        }

        public async Task<T> PostAsync<T>(string clientSource, string url, object body, string token, TokenTypeEnum tokenType, int timeOut = 15000)
        {
            T data = default;

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var res = await PostDataAsync<ApiResponse<T>>(clientSource, url, body, true, token, tokenType, timeOut);
                    if (res != null)
                    {
                        data = res.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi PostAsync with clientSource: {clientSource}, url {url}, body: {JsonConvert.SerializeObject(body)}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
                //TelegramDeployHelper.SentError($"ServiceCommunication.PostAsync() clientSource: {clientSource}", ex, "url: {0}, body: {1}", new object[] { url, body });
            }

            return data;
        }

        public async Task<T> PostAsync<T>(string clientSource, string url, object body, bool withToken = true, int timeOut = 15000)
        {
            T data = default;

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var res = await PostDataAsync<ApiResponse<T>>(clientSource, url, body, withToken, "", TokenTypeEnum.Bearer, timeOut);
                    if (res != null)
                    {
                        data = res.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi PostAsync with clientSource: {clientSource}, url {url}, body: {JsonConvert.SerializeObject(body)}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
                // TelegramDeployHelper.SentError($"ServiceCommunication.PostAsync() clientSource: {clientSource}", ex, "url: {0}, body: {1}", new object[] { url, body });
            }

            return data;
        }

        public async Task<T> PostAsyncNew<T>(string clientSource, string url, object body, bool withToken = true)
        {
            var data = default(T);

            if (!string.IsNullOrEmpty(url))
            {
                return await PostDataAsync<T>(clientSource, url, body, withToken);
            }
            return data;
        }

        public async Task<T> PostDataAsync<T>(string clientSource, string url, object data, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer, int timeOut = 15000)
        {
            if (data == null) return default;

            string content = string.Empty;

            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (withToken)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        token = _httpContextAccessor.HttpContext.GetRequestToken();
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Remove("Authorization");
                        switch (tokenType)
                        {
                            case TokenTypeEnum.Bearer:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;

                            case TokenTypeEnum.Basic:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
                                break;

                            default:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;
                        }
                    }
                }
                var cts = new CancellationTokenSource();
                cts.CancelAfter(timeOut);

                // Serialize request data
                content = JsonConvert.SerializeObject(data);

                HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

                // Call Post URL
                var postResult = await _httpClient.PostAsync(url, c, cts.Token);

                return await ConvertToDataObject<T>(postResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{clientSource} Lỗi ServiceCommunication.PostDataAsync() 1 với url {url}, data: {content}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
             //   TelegramDeployHelper.SentError($"{clientSource} gọi ServiceCommunication.PostDataAsync()", ex, "url: {0}, data: {1}", new object[] { url, content });

                return default;
            }
        }

        public Task<PostStatusData<T>> PostStatusAsync<T>(string clientSource, string url, object body, bool withToken = true, int timeOut = 15000)
        {
            throw new NotImplementedException();
        }

        public async Task<T> PutAsync<T>(string clientSource, string url, object body, bool withToken = true)
        {
            var data = default(T);

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var res = await PutDataAsync<ApiResponse<T>>(clientSource, url, body, withToken);
                    if (res != null)
                    {
                        data = res.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi PutAsync với url {url}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
               // TelegramDeployHelper.SentError($"ServiceCommunication.PutAsync()", ex, "url: {0}, body: {1}", new object[] { url, body });
            }

            return data;
        }

        public async Task<T> PutDataAsync<T>(string clientSource, string url, object data, bool withToken = true, string token = "", TokenTypeEnum tokenType = TokenTypeEnum.Bearer)
        {
            if (data == null) return default;
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (withToken)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        token = _httpContextAccessor.HttpContext.GetRequestToken();
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Remove("Authorization");
                        switch (tokenType)
                        {
                            case TokenTypeEnum.Bearer:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;

                            case TokenTypeEnum.Basic:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
                                break;

                            default:
                                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                                break;
                        }
                    }
                }
                var cts = new CancellationTokenSource();
                cts.CancelAfter(MILISECOND_TIMEOUT);

                // Serialize request data
                var content = JsonConvert.SerializeObject(data);
                HttpContent c = new StringContent(content, Encoding.UTF8, "application/json");

                // Call Post URL
                var postResult = await _httpClient.PutAsync(url, c, cts.Token);

                return await ConvertToDataObject<T>(postResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi PutDataAsync, clientSource:{clientSource}, url {url}, body: {JsonConvert.SerializeObject(data)}: {ex.Message}");

                // Nếu có lỗi => gửi cảnh báo tới Telegram
               // TelegramDeployHelper.SentError($"ServiceCommunication.PutDataAsync() clientSource:{clientSource}", ex, "url: {0}, data: {1}", new object[] { url, data });

                return default;
            }
        }

        /// <summary> Hàm convert HttpResponse sang Object nào đó </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="postResult"> </param>
        /// <returns> </returns>
        private async Task<T> ConvertToDataObject<T>(HttpResponseMessage postResult)
        {
            T blResponse = default(T);

            string data = string.Empty;

            try
            {
                // Check return data
                if (postResult == null) return default(T);

                // Reading Content in return data
                var resultContent = postResult.Content;

                // Check content data
                if (resultContent == null) return default(T);

                // Read data as String
                var finalContent = data = await resultContent.ReadAsStringAsync();

                // Check data not null or empty
                if (string.IsNullOrEmpty(finalContent)) return default(T);

                // Serialize data to Type T
                blResponse = JsonConvert.DeserializeObject<T>(finalContent);

            }
            catch (Exception ex)
            {
                _logger.LogError($"ServiceCommunication.ConvertToDataObject(), data: {data} có lỗi {ex.Message}");
            }

            return blResponse;
        }
    }
}
