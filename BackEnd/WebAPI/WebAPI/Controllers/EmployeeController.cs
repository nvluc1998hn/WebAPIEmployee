
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmployeeAPI.Controllers;
using EmployeeManagement.Common.Constant;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.Database.Context.Models;
using Microsoft.AspNetCore.Http;

namespace EmployeeAPI.Controllers
{
    /// <summary>
    ///    Thêm sửa xóa nhân viên
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 6/30/2022 created
    /// </Modified>
    [Route("api/employee")]
    public class EmployeeController : BaseController
    {
        Regex regexPhone = new Regex(@"^[0-9]{10,11}$"); // Số điện thoại chỉ được 10- 11 chữ số
        Regex regexMail = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
        private readonly IEmployeeService _userService;
        private readonly ILogger<EmployeeController> _logger;
        private string error_DisconnectServe = "Có lỗi phát sinh từ server !";

        private ResultResponse _resultResponse = new ResultResponse();

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService userService, IConfiguration configuration)
           : base(logger, configuration)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>Thêm sửa nhân viên</summary>
        /// <param name="nhanVien">Đối tượng nhân viên</param>
        /// <returns>
        ///   200 => success = true
        ///   500 => success = false, và kèm thông báo lý do thất bại
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/24/2022 created
        /// </Modified>
        [Authorize]
        [HttpPost("SaveEmployee")]
        public ActionResult<ResultResponse> SaveEmployee([FromBody] Employee employee)
        {
            string messerError = "";
            bool isExistsEmail = false;
            bool isExistsPhone = false;
            DateTime dateTime = DateTime.Now;
            bool kt = string.IsNullOrEmpty(employee.Email);


            if (string.IsNullOrEmpty(messerError) && string.IsNullOrEmpty(employee.FullName))
            {
                messerError = "Nhân viên không được bỏ trống !";
            }
            else if (messerError.Equals("") && string.IsNullOrEmpty(employee.Phone))
            {
                messerError = "Số điện thoại không được bỏ trống !";
            }
            else if (!regexPhone.IsMatch(employee.Phone) && string.IsNullOrEmpty(messerError))
            {
                messerError = "Số điện thoại không đúng định dạng !";

            }
            else if (string.IsNullOrEmpty(messerError) && string.IsNullOrEmpty(employee.PassWord))
            {
                messerError = "Mật khẩu không được bỏ trống !";
            }
            else if (string.IsNullOrEmpty(messerError) && dateTime.Year - employee.DateOfBirth.Value.Year < 18)
            {
                messerError = "Tuổi phải lớn hơn 18 !";
            }
            
            if (messerError.Equals(""))
            {
                isExistsEmail = _userService.CheckExistsEmail(employee.Email, employee.EmployeeID);

            }
            if (isExistsEmail == true)
            {
                messerError = "Tài khoản email đã tồn tại trong hệ thống !";
            }
            else if (isExistsEmail == false && messerError.Equals(""))
            {
                isExistsPhone = _userService.CheckExistsPhone(employee.Phone, employee.EmployeeID);

            }
            if (isExistsPhone == true)
            {
                messerError = "Số điện thoại đã tồn tại trong hệ thống !";
            }
            bool checkSave = false;
            if (isExistsPhone == false && isExistsEmail == false && messerError.Equals("")) 
            {
                if (employee.EmployeeID == Guid.Empty)
                {
                    Guid g = Guid.NewGuid();
                    employee.CreateDate = dateTime;
                    employee.EmployeeID = g;
                    checkSave = _userService.AddEmployee(employee);
                }
                else
                 {
                    employee.ModifiedDate = dateTime;
                    checkSave=  _userService.SaveEditEmployee(employee);

                }
                if (checkSave)
                {
                    _resultResponse.StatusCode = (int)EmployeeManagement.Common.Constant.Enum.StatusCode.Success;
                    _resultResponse.Success = true;
                }
                else
                {
                    _resultResponse.StatusCode = (int)EmployeeManagement.Common.Constant.Enum.StatusCode.False;
                    _resultResponse.Success = false;
                }
               
            }
            else
            {
                _resultResponse.Success = false;
                _resultResponse.Messenger = messerError;
            }

            if (_resultResponse.Success) return _resultResponse;
            return BadRequest(_resultResponse);

        }
        /// <summary>Lấy ra danh sách nhân viên có phân trang</summary>
        /// <param name="pageNo">Đang ở trang nào</param>
        /// <param name="pageSize">Số lượnng bản ghi 1 trang</param>
        /// <param name="sortOrder">Sắp xếp theo tiêu chí nào (chưa dùng)</param>
        /// <param name="dfrom">Điều kiện lọc từ ngày sinh</param>
        /// <param name="dto">Điều kiện lọc đến ngày sinh</param>
        /// <param name="sex">Điều kiện lọc Giới tính.</param>
        /// <param name="keyWord">Điều kiện lọc tìm kiếm theo tên,email.</param>
        /// <returns>
        ///   - 200  = > danh sách nhân viên , success= true 
        ///   - 400 => success = false, kèm thông báo lý do thất bại
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/24/2022 created
        /// </Modified>
        /// 

        [Authorize]
        [HttpGet("GetAllEmployee")]
        public ActionResult<ResultResponse> GetAllEmployee(int pageNo, int pageSize, string sortOrder, bool descyn, DateTime dfrom, DateTime dto, int sex, string keyWord)
        {

            if (dfrom.Year > 1900 && dto.Year > 1900 && dfrom > dto)
            {
                _resultResponse.Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc";
                _resultResponse.Success = false;
                _resultResponse.Result = null;
            }
            else
            {
                try
                {
                    _resultResponse.Result = _userService.GetAllEmployee(pageNo, pageSize, sortOrder, descyn, dfrom, dto, sex, keyWord).ToList();
                    _resultResponse.TotalItems = _userService.GetCountEmployee(dfrom, dto, sex, keyWord);
                }
                catch (Exception ex)
                {

                    _logger.LogError("GetAllEmployee: " + ex.Message);
                }
               
                if (_resultResponse.Result != null)
                {
                    _resultResponse.Success = true;
                }
                else
                {
                    _resultResponse.Success = false;
                    _resultResponse.Messenger = error_DisconnectServe;
                    _resultResponse.StatusCode = (int)EmployeeManagement.Common.Constant.Enum.StatusCode.False;
                }
            }
            if (_resultResponse.Success)
            {
                return _resultResponse;

            }
            return BadRequest(_resultResponse);

        }
       
        /// <summary>Xóa nhân viên.</summary>
        /// <param name="idNhanVien">ID của nhân viên.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/29/2022 created
        /// </Modified>
        /// 
        [Authorize]
        [HttpPost("DeleteEmployee")]
        public ActionResult<ResultResponse> DeleteEmployee([FromBody] Guid idEmployee)
        {

            var result = _userService.DeleteEmployee(idEmployee);
            if (result == true)
            {
                _resultResponse.Result = result;
                _resultResponse.Success = true;
                _resultResponse.StatusCode = (int)EmployeeManagement.Common.Constant.Enum.StatusCode.Success;
            }
            else
            {
                _resultResponse.Result = null;
                _resultResponse.Success = false;
                _resultResponse.StatusCode = (int)EmployeeManagement.Common.Constant.Enum.StatusCode.False;

            }

            if (_resultResponse.Success)
            {
                return _resultResponse;
            }
            return BadRequest(_resultResponse);
        }

        /// <summary>
        ///   <para>
        ///     <br />
        ///   </para>
        ///   <para>Lấy thông tin nhân viên
        /// </para>
        /// </summary>
        /// <param name="idEmployee">Id nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        [Authorize]
        [HttpGet("GetEmployee")]
        public ActionResult<ResultResponse> GetEmployee(Guid idEmployee)
        {
            var result = _userService.GetEmployeeById(idEmployee);
            if (result != null)
            {
                _resultResponse.Result = result;
                _resultResponse.Success = true;
                _resultResponse.StatusCode = (int)EmployeeManagement.Common.Constant.Enum.StatusCode.Success;
            }
            else
            {
                _resultResponse.Result = result;
                _resultResponse.Success = false;
                _resultResponse.StatusCode = (int)EmployeeManagement.Common.Constant.Enum.StatusCode.False;

            }
            if (!_resultResponse.Success)
            {
                return BadRequest(_resultResponse);
            }
            return _resultResponse;
        }
    }
}

