using AutoMapper;
using EmployeeManagement.Common.Library;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    /// <summary>
    ///    Thêm,sửa,xóa,tìm kiếm nhân viên
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
        private readonly ApplicationDbContext _db;

        public EmployeeService(IConfiguration configuration, IMapper mapper, IEmployeeRepository employeeRepository, ApplicationDbContext applicationDbContext, ILogger<EmployeeService> logger)
        {

            _configuration = configuration;
            _mapper = mapper;
            _EmployeeRepository = employeeRepository;
            _db = applicationDbContext;
            _logger = logger;
           
        }

        /// <summary>Thêm mới nhân viên.</summary>
        /// <param name="employee">Đối tượng nhân viên</param>
        /// <returns>
        ///   true - > thêm thành công ,
        ///   false - > thêm thất bại
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/30/2022 created
        /// </Modified>
        public bool AddEmployee(Employee employee)
        {
            bool isSuccess = false;
            try
            {
                employee.PassWord = Encryptor.SHA256(employee.PassWord);
                var id=  _EmployeeRepository.Insert(employee, nameof(Employee.EmployeeID));
                if (id != Guid.Empty)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("AddEmployee: " + ex.Message);               
                isSuccess = false;
            }        
            return isSuccess;
        }

        /// <summary>Sửa nhân viên</summary>
        /// <param name="Employee">Đối tượng nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 10-07-2022 created
        /// </Modified>
        public bool SaveEditEmployee(Employee Employee)
        {
            bool isSuccess = false;
            Task t = Task.Run(() =>
            {
                try
                {
                    isSuccess = _EmployeeRepository.Update(Employee);
                }
                catch (Exception ex)
                {
                    _logger.LogError("SaveEditEmployee: " + ex.Message);
                }
            });
            TimeSpan ts = TimeSpan.FromSeconds(3);
            if (!t.Wait(ts))
            {
                isSuccess = true;
            }           
            return isSuccess;
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
            Task t = Task.Run(async () =>
            {
                try
                {
                    Employee user = new Employee();
                    if (idEmployee == Guid.Empty)
                    {
                        user =  _EmployeeRepository.FindSingle(c => c.Email.ToUpper().Trim() == email.ToUpper().Trim() && !c.Email.Equals("") && !c.IsDelete);
                    }
                    else
                    {
                        user =   _EmployeeRepository.FindSingle(c => c.Email.ToUpper().Trim() == email.ToUpper().Trim() && c.EmployeeID != idEmployee && !c.Email.Equals("") && !c.IsDelete);
                    }
                    if (user != null) isExistsEmail = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError("CheckExistsEmail: " + ex.Message);
                }
            });
            TimeSpan ts = TimeSpan.FromSeconds(3);
            if (!t.Wait(ts))
            {
                isExistsEmail = false;
            }
           
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
            Task t = Task.Run(() =>
            {
                try
                {
                    Employee user = new Employee();
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
            });
            TimeSpan ts = TimeSpan.FromSeconds(3);
            if (!t.Wait(ts))
            {
               isExistsPhone = false;
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
            Task t = Task.Run(() =>
            {
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
            });
            TimeSpan ts = TimeSpan.FromSeconds(3);
            if (!t.Wait(ts))
            {
                isSuccess = false;
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
        public Employee GetEmployeeByUserName(string userName)
        {
            Employee employee = new Employee();
            Task t = Task.Run(() =>
            {
                try
                {
                    employee = _EmployeeRepository.FindSingle(c => c.Email.Trim().ToUpper() == userName || c.Phone == userName);

                }
                catch (Exception ex)
                {
                    _logger.LogError("GetIdEmployeeByUserName: " + ex.Message);
                }
            });
            TimeSpan ts = TimeSpan.FromSeconds(3);
            if (!t.Wait(ts))
            {
                employee = null;
            }
            return employee;
        }

        /// <summary>Xóa nhân viên</summary>
        /// <param name="idEmployee">Id nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 10-07-2022 created
        /// </Modified>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public bool DeleteEmployee(Guid idEmployee)
        {
            bool isDelete = false;
            Task t = Task.Run(() =>
            {
                try
                {
                    var user = _EmployeeRepository.FindSingle(c => c.EmployeeID == idEmployee);
                    if (user != null)
                    {
                        user.IsDelete = true;
                        isDelete= _EmployeeRepository.Update(user);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("DeleteEmployee: " + ex.Message);
                }
            });
            TimeSpan ts = TimeSpan.FromSeconds(3);
            if (!t.Wait(ts))
            {
                isDelete = false;
            }          
            return isDelete;
        }

        /// <summary>Lấy thông tin của Nhân Viên theo ID</summary>
        /// <param name="idEmployee">Id nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 10-07-2022 created
        /// </Modified>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public Employee GetEmployeeById(Guid idEmployee)
        {
            var user = new Employee();
            Task t = Task.Run(() =>
            {
                try
                {
                    user = _EmployeeRepository.FindSingle(c => c.EmployeeID == idEmployee);

                }
                catch (Exception ex)
                {
                    _logger.LogError("GetEmployeeById: " + ex.Message);
                }
            });
            TimeSpan ts = TimeSpan.FromSeconds(3);
            if (!t.Wait(ts))
            {
                user = null;
            }
            return user;
        }
      
        /// <summary>Lấy ra danh sách nhân viên có phân trang</summary>
        /// <param name="pageNo">Đang ở trang nào</param>
        /// <param name="pageSize">Số lượnng bản ghi 1 trang</param>
        /// <param name="sortOrder">Sắp xếp theo cột nào</param>
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
        public IEnumerable<Employee> GetAllEmployee(int pageNo, int pageSize, string sortOrder, bool descyn, DateTime dfrom, DateTime dto, int sex, string keyWord)
        {
            IEnumerable<Employee> listEmployee = new List<Employee>();          
                if (String.IsNullOrEmpty(keyWord)) keyWord = ""; // Xử lý trường hợp lỗi trong function nếu gửi về null
                var paramPageNo = new SqlParameter("PageNo", pageNo);
                var paramPageSize = new SqlParameter("PageSize", pageSize);
                var paramSortOrder = new SqlParameter("SortOrder", sortOrder);
                var typeSort = new SqlParameter("descyn", descyn);
                var paramdfrom = new SqlParameter("dfrom", dfrom);
                var paramdto = new SqlParameter("dto", dto);
                var paramsex = new SqlParameter("sex", sex);
                var paramkeyWord = new SqlParameter("keyWord", keyWord);
               
                Task t = Task.Run(() =>
                {
                    try
                    {
                        listEmployee = _db.Employee.FromSqlRaw("EXEC WebAPI_Employee_GetAll @PageNo,@PageSize,@SortOrder,@descyn,@dfrom,@dto,@sex,@keyWord",
                         paramPageNo, paramPageSize, paramSortOrder, typeSort, paramdfrom, paramdto, paramsex, paramkeyWord);
                       
                        
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("GetAllEmployee: " + ex.Message);
                    }
                });
                TimeSpan ts = TimeSpan.FromSeconds(3);
                if (!t.Wait(ts))
                {

                    listEmployee = null;
                }      
            return listEmployee;

        }

        /// <summary>Lấy số lượng bản ghi theo điều kiện lọc không phân trang</summary>
        /// <param name="dfrom">Từ ngày sinh</param>
        /// <param name="dto">Đến ngày sinh</param>
        /// <param name="sex">Giới tính</param>
        /// <param name="keyWord">Từ khóa tìm kiếm</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 10-07-2022 created
        /// </Modified>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public int GetCountEmployee(DateTime dfrom, DateTime dto, int sex, string keyWord)
        {
            int number_Record = 0;

            if (String.IsNullOrEmpty(keyWord)) keyWord = ""; // Xử lý trường hợp lỗi trong function nếu gửi về null
            var paramdfrom = new SqlParameter("dfrom", dfrom);
            var paramdto = new SqlParameter("dto", dto);
            var paramsex = new SqlParameter("sex", sex);
            var paramkeyWord = new SqlParameter("keyWord", keyWord);

            var output = new SqlParameter();
            output.ParameterName = "@parameterReturn";
            output.SqlDbType = SqlDbType.Int;
            output.Direction = ParameterDirection.Output;

            try
            {
                _db.Database.ExecuteSqlRaw("EXEC WebAPI_CountEmployee @dfrom={0},@dto={1},@sex={2},@keyword={3}, @employee_count={4} OUT"
                        , paramdfrom, paramdto, paramsex, paramkeyWord, output);
                number_Record = (int)output.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetCountEmployee: " + ex.Message);
            }
            return number_Record;
        }


    }
}
