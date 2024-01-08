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
using Enum = EmployeeManagement.Common.Constant.Enum;

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
        ///   Trả về thông ID của Employee, Token
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        // lucnv 6/29/2022 created
        /// </Modified>
        [HttpPost("login")]
        public ActionResult<ResultResponse> Login([FromBody] Employee employee)
        {

            var result = new ResultResponse();
            bool checkLogin = false;
            // ưu tiên check trong file config trước , nếu không có thì check trong db
            string path = @"c:\Users\lucnv\Desktop\pass.txt";
            if (System.IO.File.Exists(path))
            {
                try
                {
                    using (StreamReader sr = new StreamReader("c:/Users/lucnv/Desktop/pass.txt"))
                    {
                        string line = "";
                        string[] taikhoan = new string[2];
                        int i = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            taikhoan[i] = line;
                            i++;
                        }
                        if (taikhoan[0].Trim() == employee.Email && taikhoan[1].Trim() == employee.PassWord)
                        {
                            checkLogin = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Login Read File :" + ex.Message);
                }

            }
            if (!checkLogin)
            {
               checkLogin = _userService.CheckLogin(employee.Email, employee.PassWord);
            }
            UserInfo userInfo = new UserInfo();
            if (checkLogin)
            {
                Employee inforEmployee = new Employee();

                userInfo.AccessToken = SaveSession();
                
                userInfo.FullName = "Nguyễn Trọng Tú";
                
                userInfo.EmployeeId = Guid.NewGuid();

                result.StatusCode = (int)Enum.StatusCode.LoginSucces;
                result.Result = userInfo;
                result.Success = true;
            }
            else
            {
                result.StatusCode = (int)Enum.StatusCode.LoginSucces;
                result.Messenger = Enum.messageCode.LoginFailed.ToString();
                result.Result = null;
                result.Success = false;

            }
            return result;
        }


    }
}
