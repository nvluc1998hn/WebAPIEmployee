using Base.Common.Helper;
using Base.Common.Jwt.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt
{
    /// <summary>
    /// Middle ware valid token
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 14/08/2024 created
    /// </Modified>
    /// <seealso cref="Microsoft.AspNetCore.Http.IMiddleware" />
    public class JwtTokenValidatorMiddleware : IMiddleware
    {
        private readonly IJwtService _jwtService;
        private readonly ILogger<JwtTokenValidatorMiddleware> _logger;

        public JwtTokenValidatorMiddleware(IJwtService jwtService, ILogger<JwtTokenValidatorMiddleware> logger)
        {
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var isAuthen = await _jwtService.ValidateToken(context);
                if (isAuthen)
                {
                   await next(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()} có lỗi khi xác thực: {ex}");
            }
        }
    }
}
