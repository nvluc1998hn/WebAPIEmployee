using Base.Common.Cache.Redis.Interface;
using Base.Common.Helper;
using Base.Common.Jwt.Claims;
using Base.Common.Jwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Base.Common.Jwt.Service
{
    public class JwtService : IJwtService
    {
        private static readonly ISet<string> DefaultClaims = new HashSet<string>
        {
            JwtClaimsTypes.Sub,
            JwtClaimsTypes.Jti,
            JwtClaimsTypes.Iat,
        };

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
        private JwtOptions Options { get; }
        private readonly IInstanceCache _cache;
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string ServerIP = "192.168.1.1";

        public JwtService(JwtOptions options, IInstanceCache cache, IHttpContextAccessor httpContextAccessor)
        {
            Options = options;
            _httpContextAccessor = httpContextAccessor;

            _cache = cache;

            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.SecretKey));

            _signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

            _tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = issuerSigningKey,
                ValidateAudience = Options.ValidateAudience,
                ValidAudience = Options.ValidAudience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            _jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
        }

        public JsonWebToken CreateToken(Guid userId, int xnCode, string customerCode = null, Guid? loginUserId = null)
        {
            JsonWebToken? token = null;

            var methodPath = $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (userId.Equals(Guid.Empty))
                {
                    Log.Logger.Fatal($"{methodPath}: userId không hợp lệ!");
                }
                else
                {
                    var now = DateTime.Now;

                    // Ngày hết hạn
                    var expires = now.AddMinutes(Options?.ExpiredMinutes ?? 240);

                    List<Claim> claims = new()
                    {
                        new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new (JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
                        new (JwtRegisteredClaimNames.Sub, userId.ToString()),
                        new (JwtClaimsTypes.XnCode,xnCode.ToString()),
                        new (JwtClaimsTypes.CustomerCode, customerCode)
                    };

                    if (loginUserId.HasValue && !Guid.Empty.Equals(loginUserId) && !userId.Equals(loginUserId))
                    {
                        claims.Add(new(JwtClaimsTypes.LoginUserId, loginUserId.ToString()));
                    }
                    var jwt = new JwtSecurityToken(
                        issuer: ServerIP,
                    claims: claims,
                        expires: expires,
                        signingCredentials: _signingCredentials
                    );

                    var accessToken = _jwtSecurityTokenHandler.WriteToken(jwt);

                    token = new JsonWebToken
                    {
                        AccessToken = accessToken,
                        RefreshToken = GenerateRefreshToken(),
                        Expires = expires,
                        Id = userId.ToString(),
                        Claims = claims.ToDictionary(c => c.Type, c => c.Value)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal($"{methodPath} có lỗi: {ex}");
            }

            return token;
        }

        /// <summary> Generates the refresh token. https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/ </summary>
        /// <returns> </returns>
        /// <Modified> Name Date Comments trungtq 06/09/2023 created </Modified>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task DeactivateAsync(string token)
        {
            await Task.Delay(0);
            _cache.Remove(GetKey(token));
        }

        public async Task<bool> IsCurrentActiveToken() => await IsActiveAsync(GetContextToken());


        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetContextToken());

        public JsonWebTokenPayload GetTokenPayload(string accessToken)
        {
            JsonWebTokenPayload payload = null;

            try
            {
                // Nếu token hợp lệ mới xử lý tiếp
                if (!string.IsNullOrEmpty(accessToken))
                {
                    _jwtSecurityTokenHandler.ValidateToken(accessToken, _tokenValidationParameters, out var validatedSecurityToken);

                    if (validatedSecurityToken is JwtSecurityToken jwt)
                    {
                        payload = new JsonWebTokenPayload
                        {
                            Subject = jwt.Subject,
                            Expires = jwt.ValidTo.ToLocalTime(),
                            Claims = jwt.Claims.Where(x => !DefaultClaims.Contains(x.Type)).ToDictionary(k => k.Type, v => v.Value)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}() có lỗi: {ex}");
            }

            return payload;
        }

        public Task<bool> IsActiveAsync(string token)
        {
            var cached = _cache.Get<string>(GetKey(token));

            return Task.FromResult(string.IsNullOrEmpty(cached));
        }

        public async Task<bool> ValidateToken(HttpContext context = null)
        {
            bool valid = false;

            try
            {
                var httpContext = context ?? _httpContextAccessor?.HttpContext;
                if (httpContext != null)
                {
                    var endpoint = context.GetEndpoint();

                    var isAllowAnonymous = endpoint?.Metadata != null && endpoint.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));

                    // Kiểm tra attribute hoặc url có nằm trong ds bỏ qua xác thực hay không
                    var skip = isAllowAnonymous || Options.ExceptPattern?.Any(path => httpContext.Request.Path.Value.Contains(path?.Trim(), StringComparison.InvariantCultureIgnoreCase)) == true;

                    if (skip)
                    {
                        valid = true;
                    }
                    else
                    {
                        var token = httpContext.GetRequestToken();
                        var paypload = GetTokenPayload(token);

                        // Kiểm tra payload
                        if (paypload == null)
                        {
                            Log.Logger.Fatal($"Token không hợp lệ: {token}");
                        }
                        // Kiểm tra thời hạn token
                        else if (paypload.Expires <= DateTime.Now)
                        {
                            Log.Logger.Fatal($"Token đã hết hạn: {token}");
                        }
                        else
                        {
                            // Kiểm tra và lấy thông tin user gán vào context để sử dụng
                            var handler = new HttpClientHandler
                            {
                                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                            };

                            using HttpClient httpClient = new(handler);

                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            httpClient.DefaultRequestHeaders.Remove("User-Agent");
                            httpClient.DefaultRequestHeaders.Add("User-Agent", "BA JWT Service");
                            httpClient.DefaultRequestHeaders.Remove("Authorization");
                            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                            // var url = Singleton<UserCrossServiceManager>.Instance.GetApiUrl(CrossServiceUrlKeyNames.Authen);

                            var url = "duong dan login";

                            // Lấy body theo url 
                            var content = await httpClient.GetStringAsync(url);

                            var dictUserAuthen = JsonConvert.DeserializeObject<Dictionary<Guid, UserAuthenModel>>(content);

                            if (dictUserAuthen?.Count > 0)
                            {
                                foreach (var user in dictUserAuthen)
                                {
                                    httpContext.Items[user.Key] = user.Value;
                                }

                                valid = true;
                            }
                            else
                            {
                                Log.Logger.Fatal($"Không lấy được thông tin user với token: {token}");
                            }
                        }
                    }
                }
                else
                {
                    Log.Logger.Fatal($"Context không hợp lệ");
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal($"{GetType().Name}.{MethodHelper.GetNameAsync()} có lỗi: {ex}");
            }

            return valid;
        }

        private string GetContextToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Request.Path.StartsWithSegments("/signalr"))
            {
                var accessToken = httpContext.Request.Query["access_token"];

                return accessToken;
            }

            var authorizationHeader = httpContext.Request.Headers["authorization"];

            return string.IsNullOrEmpty(authorizationHeader)
                ? string.Empty
                : authorizationHeader.Single().Split(' ').Last();
        }

        private static string GetKey(string token) => $"tokens:{token}".ToUpper();
    }
}
