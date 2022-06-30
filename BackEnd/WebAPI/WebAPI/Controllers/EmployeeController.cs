
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
    ///   Xử lý các nhiệp vụ như thêm sửa xóa nhân viên
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

        private ResultResponse _resultResponse = new ResultResponse();
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService userService, IConfiguration configuration)
           : base(logger, configuration)
        {
            _userService = userService;
        }

        /// <summary>Thêm sửa nhân viên</summary>
        /// <param name="nhanVien">Đối tượng nhân viên</param>
        /// <returns>
        ///   200 => success = true
        ///   400 => success = false, và kèm thông báo lý do thất bại
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/24/2022 created
        /// </Modified>
        [Authorize]
        [HttpPost("addOrUpdateEmployee")]
        public async ActionResult<ResultResponse> addOrUpdateEmployee([FromBody] Employee employee)
        {
            string messerError = "";
            bool isExistsEmail = false;
            bool isExistsPhone = false;
            DateTime dateTime = DateTime.Now;

            try {
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
                else if (string.IsNullOrEmpty(messerError) && !regexMail.IsMatch(employee.Email) && !string.IsNullOrEmpty(employee.Email))
                {
                    messerError = "Email không hợp lệ !";
                }
                if (messerError.Equals(""))
                {
                    isExistsEmail = _userService.CheckExistsEmail(employee.Email, employee.EmployeeID);

                }
                if (isExistsEmail)
                {
                    messerError = "Tài khoản email đã tồn tại trong hệ thống !";
                }
                else if (!isExistsEmail && messerError.Equals(""))
                {
                    isExistsPhone = _userService.CheckExistsPhone(employee.Phone, employee.EmployeeID);

                }
                if (isExistsPhone)
                {
                    messerError = "Số điện thoại đã tồn tại trong hệ thống !";
                }
                // Gửi về trạng thái là đang thêm mới hay sửa
                if (!isExistsPhone && !isExistsEmail && messerError.Equals(""))
                {
                    if (employee.EmployeeID == Guid.Empty)
                    {
                        Guid g = Guid.NewGuid();
                        employee.CreateDate = dateTime;
                        employee.EmployeeID = g;
                        _userService.AddEmployee(employee);
                    }
                    else
                    {
                        employee.ModifiedDate = dateTime;
                         _userService.SaveEditEmployee(employee);

                    }
                    _resultResponse.Success = true;
                }
                else
                {
                    _resultResponse.Success = false;
                    _resultResponse.messenger = messerError;
                }
            }
            catch(Exception ex){
                _logger.LogError("addOrUpdateEmployee: " + ex.Message);
            }
            return  _resultResponse;
        }
        /// <summary>Lấy ra danh sách nhân viên có phân trang</summary>
        /// <param name="PageNo">Đang ở trang nào</param>
        /// <param name="PageSize">Số lượnng bản ghi 1 trang</param>
        /// <param name="SortOrder">Sắp xếp theo tiêu chí nào (chưa dùng)</param>
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
        [Authorize]
        [HttpGet("getAllEmployee")]
        public ActionResult<ResultResponse> getAllEmployee(int PageNo, int PageSize, string SortOrder,bool descyn, DateTime dfrom, DateTime dto, int sex, string keyWord)
        {
            try {
                if (dfrom.Year > 1900 && dto.Year > 1900 && dfrom > dto)
                {
                    _resultResponse.messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc";
                    _resultResponse.Success = false;
                    _resultResponse.Result = null;
                }
                else
                {
                    var result = _userService.GetAllEmployee(PageNo, PageSize, SortOrder, descyn, dfrom, dto, sex, keyWord);
                    _resultResponse.Result = result;
                    _resultResponse.Success = true;
                }          
            }
            catch (Exception ex)
            {
                _logger.LogError("getAllEmployee: " + ex.Message);
            }
            if (_resultResponse.messenger != "")
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
        [Authorize]
        [HttpPost("deleteEmployee")]
        public ActionResult<ResultResponse> deleteEmployee([FromBody] Guid idNhanVien)
        {
            try
            {
                var result = _userService.DeleteEmployee(idNhanVien);
                if (result)
                {
                    _resultResponse.Result = result;
                    _resultResponse.Success = true;
                }
                else
                {
                    _resultResponse.Result = null;
                    _resultResponse.Success = false;
                    _resultResponse.messenger = "Nhân Viên không tồn tại trong hệ thống !";

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("deleteEmployee: " + ex.Message);
            }         
            if (_resultResponse.Success)
            {
                return _resultResponse;
            }
            return BadRequest();
        }
        [Authorize]
        [HttpGet("getEmployee")]
        public ActionResult<ResultResponse> getEmployee(Guid idNhanVien)
        {
            try
            {
                var result = _userService.GetEmployeeById(idNhanVien);
                if (result != null)
                {
                    _resultResponse.Result = result;
                    _resultResponse.Success = true;
                }
                else
                {
                    _resultResponse.messenger = "Nhân viên không tồn tại trong hệ thống!";
                    _resultResponse.Result = result;
                    _resultResponse.Success = false;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("getEmployee: " + ex.Message);

            }
            if (!_resultResponse.Success)
            {
                return BadRequest(_resultResponse);
            }
            return _resultResponse;
        }
        /// <summary>Lấy ra số lượng bản ghi dùng cho phân trang.</summary>
        /// <returns>
        ///   Số lượng bản ghi
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/24/2022 created
        /// </Modified>
        [HttpGet("GetCountEmployee")]
        public ResultResponse GetCountEmployee()
        {
            try {
                var result = _userService.GetCountEmployee();
                _resultResponse.Result = result;
                _resultResponse.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCountEmployee: " + ex.Message);
            }      
            return _resultResponse;
        }

    }
}

