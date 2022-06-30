
using EfCore.ViewModels;
using EmployeeManagement.Common.Library;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;
        public readonly IConfiguration _configuration;

        public BaseController(ILogger<BaseController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        #region Authentication

        protected string SaveSession()
        {          
            var token = GenerateJsonWebToken();
            return token;       
        }

        private string GenerateJsonWebToken()
        {
            var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(_configuration["Jwt:Key"].ToString()))
                .AddSubject(_configuration["Jwt:Issuer"].ToString())
                .AddIssuer(_configuration["Jwt:Issuer"].ToString())
                .Build();

            return token;
        }
        #endregion
    }
}
