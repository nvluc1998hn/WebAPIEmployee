using Admin.Application.Enums;
using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Respond;
using Admin.Domain.Entities;
using Base.Common.Helper;
using Base.Common.Jwt.Models;
using Base.Common.Jwt.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;
using Utilities.Helpers;

namespace Admin.Application.Services
{
    public class AuthenticationServiceSingleton : IAuthenticationServiceSingleton
    {
        private readonly ILogger<AuthenticationServiceSingleton> _logger;
        private readonly IAdminUserService _adminUserService;
        private readonly IJwtService _jwtService;

        public AuthenticationServiceSingleton(ILogger<AuthenticationServiceSingleton> logger, IAdminUserService adminUserService, IJwtService jwtService
           )
        {
            _jwtService = jwtService;
            _adminUserService = adminUserService;  
            _logger = logger;
        }

        public string CreateAccessToken(Guid userId)
        {
            string token = null;

            try
            {
                // Tạo token
                var jwt = _jwtService.CreateToken(userId);
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

        public async Task<LoginViewModel> CreateLoginSession(AdminUser adminUser, LoginViewModel loginInfo = null)
        {
            try
            {
                if (adminUser != null && adminUser.Id != Guid.Empty && !adminUser.IsLock)
                {
                    loginInfo.UserId = adminUser.Id;
                    loginInfo.CompanyId = adminUser.FK_Agency;

                    // Tạo token
                    loginInfo.AccessToken = CreateAccessToken(loginInfo.UserId);

                    // Có token thì lấy tiếp các thông tin khác
                    if (!string.IsNullOrWhiteSpace(loginInfo.AccessToken))
                    {
                        loginInfo.UserName = adminUser.Username;
                        loginInfo.UserType = (UserType)adminUser.UserType;
                        loginInfo.FullName = adminUser.Fullname;
                        loginInfo.PhoneNumber = adminUser.PhoneNumber;
                        loginInfo.Status = LoginStatus.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return loginInfo;
        }

        public async Task<Dictionary<Guid, UserAuthenModel>> GetUserByToken(AuthenRequest request)
        {
            Dictionary<Guid, UserAuthenModel> result = null;

            try
            {
                if (!request.UserId.Equals(Guid.Empty))
                {
                    result = new();

                    var user = await _adminUserService.GetUserByUsername(request.UserName);
                    if (user != null)
                    {
                        result[user.Id] = new UserAuthenModel()
                        {
                            UserId = user.Id,
                            UserName = user.Username,
                            UserType = user.UserType,
                          //  Permissions = await _userPermissionsService.GetPermissionsByPriority(userId),
                            CompanyId = user.FK_Agency,
                         //   CompanyType = user.Id,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                var message = $"Lỗi {GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}";
                _logger.LogError(message);
            }

            return result;
        }

        public async Task<LoginViewModel> Login(LoginRequest request)
        {
            var response = new LoginViewModel() { Status = LoginStatus.None };
            try
            {
                var loginvalid = await ValidateLogin(request.UserName, request.Password);

                if(loginvalid.status == LoginStatus.Success)
                {
                    response = await CreateLoginSession(loginvalid.user, response);
                }
                else
                {
                    response.Status = loginvalid.status;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return response;
        }

        private async Task<(LoginStatus status, AdminUser user)> ValidateLogin(string username, string password)
        {

            // Kiểm tra xem user có tồn tại trong CSDL không
            var dataByUserName = await _adminUserService.GetUserByUsername(username);

            var status = LoginStatus.None;
            await Task.Delay(0);

            try
            {
                bool isValidateSuccess = true;
               
                isValidateSuccess = dataByUserName != null && !dataByUserName.IsLock;

                if (isValidateSuccess)
                {
                    //lấy ra thông tin user
                    string hashPassWord = StringHelper.EncryptPassword(password);

                    if (!string.IsNullOrEmpty(dataByUserName.Password) && dataByUserName.Password.Equals(hashPassWord))
                    {
                        status = LoginStatus.Success;
                    }
                    else
                    {
                        status = LoginStatus.LoginFailed;
                    }
                }
                else
                {
                    status = LoginStatus.LoginFailed;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }

            return (status, dataByUserName);
        }

    }
}
