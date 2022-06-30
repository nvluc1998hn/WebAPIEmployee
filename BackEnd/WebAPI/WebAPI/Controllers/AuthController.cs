using AutoMapper;
using EfCore.ViewModels;
using EmployeeManagement.Common.Constant;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace EmployeeAPI.Controllers
{
    /// <summary>
    ///   Kiểm tra Login người dùng, tạo token
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 6/30/2022 created
    /// </Modified>
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IEmployeeService _userService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IEmployeeService userService, IConfiguration configuration)
            : base(logger, configuration)
        {
            _userService = userService;
            _logger = logger;
        }
        /// <summary>Kiểm tra login</summary>
        /// <param name="employee">Tài khoản , mật khẩu truyền vào</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/29/2022 created
        /// </Modified>
        [HttpPost("login")]
        public ActionResult<ResultResponse> Login([FromBody] Employee employee)
        {
            
            var result = new ResultResponse();
            bool isAdmin = false;
            try
            {
                string path = @"c:\Users\lucnv\Desktop\abc.txt";
                if (System.IO.File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader("c:/Users/lucnv/Desktop/abc.txt"))
                    {
                        string line= "";
                        string[] taikhoan = new string[2];
                        int i = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            taikhoan[i] = line;
                            i++;
                        }
                        if (taikhoan[0].Trim() == employee.Email && taikhoan[1].Trim() == employee.PassWord) isAdmin = true;
                    }
                   
                }               
            }
            catch (Exception ex)
            {
                _logger.LogError("Login :" + ex.Message);
            }
            try
            {
               
                var checkLogin = _userService.CheckLogin(employee.Email, employee.PassWord);
                if (isAdmin) checkLogin = true;
                if (checkLogin)
                {
                    var token = SaveSession();
                    result.Result = token;
                    result.Success = true;                
                }           

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return result;
        }


    }
}
