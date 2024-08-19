using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Base.Common.Constant;
using Base.Common.Controllers;
using Base.Common.Jwt.Claims;
using Base.Common.Jwt.Models;
using Base.Common.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Base.Common.Mvc;

namespace Admin.API.Controllers
{
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationServiceSingleton _authenService;

        public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IMediator mediator,
        IAuthenticationServiceSingleton authenService,
        IConfiguration config)
        {
            _authenService = authenService;
        }

        [HttpPost]
        [Route("login")]
        [ApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<ApiResponse> Login(LoginRequest request)
        {
            ApiResponse response;
            try
            {
                var result = await _authenService.Login(new LoginRequest() {UserName= request.UserName, Password= request.Password, IPClient= request.IPClient });
              
                response = new ApiOkResultResponse(result);
            }
            catch (Exception ex)
            {
                response = new ApiCatchResponse(ex);
            }

            return response;
        }

        /// <summary> Lấy thông tin user có trong token </summary>
        [HttpGet]
        [Route("users/authen")]
        [ApiVersion("1")]
        [ApiValidationFilterAttribute]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<Dictionary<Guid, UserAuthenModel>> GetUserByToken()
        {
            try
            {
                var request = new AuthenRequest()
                {
                    UserId = HttpContext.GetUserID(),
                    UserName = HttpContext.GetUserName(),
                    LoginUserId = HttpContext.GetLoginUserID(),
                    Token = HttpContext.GetRequestToken(),
                    SessionKey = HttpContext.GetSessionKey()
                };

                var result = await _authenService.GetUserByToken(request);

                return result;
            }
            catch
            {
                return null;
            }
        }

    }
}
