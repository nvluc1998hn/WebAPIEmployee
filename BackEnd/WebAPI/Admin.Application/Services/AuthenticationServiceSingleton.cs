using Admin.Application.Enums;
using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Respond;
using Base.Common.Helper;
using Base.Common.Jwt.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Services
{
    public class AuthenticationServiceSingleton : IAuthenticationServiceSingleton
    {
        private readonly ILogger<AuthenticationServiceSingleton> _logger;
        private readonly IServiceCommunication _serviceCommunication;
        private readonly IJwtService _jwtService;

        public AuthenticationServiceSingleton(ILogger<AuthenticationServiceSingleton> logger, IServiceCommunication serviceCommunication, IJwtService jwtService)
        {
            _serviceCommunication = serviceCommunication;
            _jwtService = jwtService;
            _logger = logger;
        }

        public string CreateAccessToken(Guid userId, int xnCode, string customerCode = null, Guid? loginUserId = null)
        {
            string token = null;

            try
            {
                // Tạo token
                var jwt = _jwtService.CreateToken(userId, xnCode, customerCode, loginUserId);
                if (jwt != null)
                {
                    token = jwt.AccessToken;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }

            return token;
        }

        public async Task<LoginViewModel> CreateLoginSession(Guid userId, Guid? loginUserId = null)
        {
            var response = new LoginViewModel() { Status = LoginStatus.None };
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return response;
        }

        public async Task<LoginViewModel> Login(LoginRequest request)
        {
            var response = new LoginViewModel() {Status = LoginStatus.None};
            try
            {
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return response;
        }
    }
}
