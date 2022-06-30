using AutoMapper;
using EfCore.ViewModels;
using EmployeeManagement.Common.Library;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    /// <summary>
    ///   Sử lý các nhiệp vụ thêm,sửa,xóa,tìm kiếm nhân viên
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 6/30/2022 created
    /// </Modified>
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        public static int number_Record = 0;
        private readonly ApplicationDbContext _db;

        public EmployeeService(IConfiguration configuration, IMapper mapper, IEmployeeRepository employeeRepository, ApplicationDbContext applicationDbContext,ILogger<EmployeeService> logger)
        {

            _configuration = configuration;
            _mapper = mapper;
            _EmployeeRepository = employeeRepository;
            _db = applicationDbContext;
            _logger = logger;
        }

        /// <summary>Adds the employee.</summary>
        /// <param name="Employee">Thêm mới nhân viên.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/30/2022 created
        /// </Modified>
        public bool AddEmployee(Employee Employee)
        {
            try
            {
                Employee.PassWord = Encryptor.SHA256(Employee.PassWord);
                _EmployeeRepository.Insert(Employee);
                _EmployeeRepository.SaveChange();
            }
            catch (Exception ex)
            {
                _logger.LogError("AddEmployee: " + ex.Message);

            }
            return true;
        }
        public bool SaveEditEmployee(Employee Employee)
        {
            try
            {
                _EmployeeRepository.Update(Employee);
                _EmployeeRepository.SaveChange();
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveEditEmployee: " + ex.Message);
            }       
            return true;
        }

        /// <summary>Kiểm tra sự tồn tại của email</summary>
        /// <param name="email">Email</param>
        /// <param name="action">Trạng thái đang là thêm hay sửa</param>
        /// <param name="idEmployee">Mã nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/24/2022 created
        /// </Modified>
        public bool CheckExistsEmail(string email, Guid idEmployee)
        {
            bool isExistsEmail = false;
            try
            {
                Employee user = new Employee();
                if (idEmployee == Guid.Empty)
                {
                    user = _EmployeeRepository.FindSingle(c => c.Email.ToUpper().Trim() == email.ToUpper().Trim() && !c.Email.Equals("") && !c.IsDelete);
                }
                else
                {
                    user = _EmployeeRepository.FindSingle(c => c.Email.ToUpper().Trim() == email.ToUpper().Trim() && c.EmployeeID != idEmployee && !c.Email.Equals("") && !c.IsDelete);
                }
                if (user != null) isExistsEmail = true;

            }
            catch (Exception ex)
            {
                _logger.LogError("CheckExistsEmail: " + ex.Message);
            }
            // Trường hợp thêm mới thì không check với chính nó
            return isExistsEmail;
        }

        /// <summary>Kiểm tra sự tồn tại của Điện thoại</summary>
        /// <param name="phone">Số điện thoại</param>
        /// <param name="action">Trạng thái</param>
        /// <param name="idEmployee">Mã nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/24/2022 created
        /// </Modified>
        public bool CheckExistsPhone(string phone, Guid idEmployee)
        {
            bool isExistsPhone = false;
            try
            {
                Employee user = new Employee();
                // Trường hợp thêm mới thì không check với chính nó
                if (idEmployee == Guid.Empty)
                {
                    user = _EmployeeRepository.FindSingle(c => c.Phone == phone && !c.IsDelete);
                }
                else
                {
                    user = _EmployeeRepository.FindSingle(c => c.Phone == phone && c.EmployeeID != idEmployee && !c.IsDelete);
                }
                if (user != null) isExistsPhone = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckExistsPhone: " + ex.Message);
            }        
            return isExistsPhone;
        }

        /// <summary>Kiểm tra thông tin đăng nhập</summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="passWord">The pass word.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/30/2022 created
        /// </Modified>
        public bool CheckLogin(string userName, string passWord)
        {
            bool isSuccess = false;
            try
            {
                var user = _EmployeeRepository.FindSingle(c => c.PassWord == Encryptor.SHA256(passWord) && (c.Email.Trim().ToUpper() == userName || c.Phone == userName));
                if (user != null)
                {
                     isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckLogin: " + ex.Message);
            }
           
            return isSuccess;
        }
        /// <summary>Lấy ID của nhân viên khi login sử dụng cho việc lưu lại log người sửa, thêm mới, xóa</summary>
        /// <param name="userName">Tài khoản đăng nhập</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 30-06-2022 created
        /// </Modified>
        public Guid GetIdEmployeeByUserName(string userName)
        {
            Guid idEmployee = Guid.Empty;
            try
            {
                 idEmployee = _EmployeeRepository.FindSingle(c =>c.Email.Trim().ToUpper() == userName || c.Phone == userName).EmployeeID;
                
            }
            catch (Exception ex)
            {
                _logger.LogError("GetIdEmployeeByUserName: " + ex.Message);
            }
            return idEmployee;
        }
        public bool DeleteEmployee(Guid idEmployee)
        {
            bool isDelete = false;
            try
            {
                var user = _EmployeeRepository.FindSingle(c => c.EmployeeID == idEmployee);
                if (user != null)
                {
                    user.IsDelete = true;
                    _EmployeeRepository.Update(user);
                    _EmployeeRepository.SaveChange();
                    isDelete = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteEmployee: " + ex.Message);

            }
            return isDelete;
        }

       

        public Employee GetEmployeeById(Guid idEmployee)
        {
            var user = new Employee();
            try
            {
                user = _EmployeeRepository.FindSingle(c => c.EmployeeID == idEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetEmployeeById: " + ex.Message);
            }
            return user;
        }

        public int GetCountEmployee()
        {
            return number_Record;
        }

        /// <summary>Lấy ra danh sách nhân viên có phân trang</summary>
        /// <param name="PageNo">Đang ở trang nào</param>
        /// <param name="PageSize">Số lượnng bản ghi 1 trang</param>
        /// <param name="SortOrder">Sắp xếp theo cột nào</param>
        /// <param name="descyn">sắp xếp tăng dần hay giảm dần</param>
        /// <param name="dfrom">Điều kiện lọc từ ngày sinh</param>
        /// <param name="dto">Điều kiện lọc đến ngày sinh</param>
        /// <param name="sex">Điều kiện lọc Giới tính.</param>
        /// <param name="keyWord">Điều kiện lọc tìm kiếm theo tên,email.</param>
        /// <returns>
        ///   Danh sách nhân viên
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/24/2022 created
        /// </Modified>
        public IEnumerable<Employee> GetAllEmployee(int PageNo, int PageSize, string SortOrder,bool descyn, DateTime dfrom, DateTime dto, int sex, string keyWord)
        {
            IEnumerable<Employee> listEmployee = new List<Employee>();
            try
            {
                if (String.IsNullOrEmpty(keyWord)) keyWord = ""; // Xử lý trường hợp lỗi trong function nếu gửi về null
                var paramPageNo = new SqlParameter("PageNo", PageNo);
                var paramPageSize = new SqlParameter("PageSize", PageSize);
                var paramSortOrder = new SqlParameter("SortOrder", SortOrder);
                var typeSort = new SqlParameter("descyn", descyn);
                var paramdfrom = new SqlParameter("dfrom", dfrom);
                var paramdto = new SqlParameter("dto", dto);
                var paramsex = new SqlParameter("sex", sex);
                var paramkeyWord = new SqlParameter("keyWord", keyWord);

                var output = new SqlParameter();
                output.ParameterName = "@parameterReturn";
                output.SqlDbType = SqlDbType.Int;
                output.Direction = ParameterDirection.Output;

                _db.Database.ExecuteSqlRaw("EXEC WebAPI_CountEmployee @dfrom={0},@dto={1},@sex={2},@keyword={3}, @employee_count={4} OUT"
                   , paramdfrom, paramdto, paramsex, paramkeyWord, output);

                number_Record = (int)output.Value;
                listEmployee = _db.Employee.FromSqlRaw("EXEC WebAPI_Employee_GetAll @PageNo,@PageSize,@SortOrder,@descyn,@dfrom,@dto,@sex,@keyWord",
                    paramPageNo, paramPageSize, paramSortOrder, typeSort, paramdfrom, paramdto, paramsex, paramkeyWord);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllEmployee: " + ex.Message);
            }
            
            return listEmployee;
        }

       
    }
}
