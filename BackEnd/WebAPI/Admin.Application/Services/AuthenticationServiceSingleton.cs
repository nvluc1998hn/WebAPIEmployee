using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Respond;
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
        private readonly IServiceCommunication _serviceCommunication

        public AuthenticationServiceSingleton(ILogger<AuthenticationServiceSingleton> logger)
        {
            _logger = logger;
        }

        public Task<LoginViewModel> CreateLoginSession(Guid userId, Guid? loginUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<LoginViewModel> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
