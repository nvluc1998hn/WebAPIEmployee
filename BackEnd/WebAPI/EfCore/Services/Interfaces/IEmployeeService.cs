using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>Kiểm tra thông tin đăng nhập</summary>
        /// <param name="userName">Tài khoản</param>
        /// <param name="passWord">Mật khẩu</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        bool CheckLogin(string userName, string passWord);

        /// <summary>Lấy thông tin của Nhân Viên theo ID</summary>
        /// <param name="idEmployee">Id nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        Employee GetEmployeeById(Guid idEmployee);

        /// <summary>Lấy thông tin nhân viên theo TK đăng nhập</summary>
        /// <param name="userName">Tên tài khoản</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        Employee GetEmployeeByUserName(string userName);


        /// <summary>Lấy thông tin nhân viên theo điều kiện lọc có phân trang</summary>
        /// <param name="pageNo">Trang số</param>
        /// <param name="pageSize">Số lượng 1 trang</param>
        /// <param name="sortOrder">Sắp xếp theo cột nào </param>
        /// <param name="descyn">Chưa dùng</param>
        /// <param name="dfrom">Điều kiện lọc từ ngày</param>
        /// <param name="dto">Điều kiện lọc đến ngày</param>
        /// <param name="sex">Điều kiện lọc giới tính</param>
        /// <param name="keyWord">Điều kiện lọc theo email hoặc tên </param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        IEnumerable<Employee> GetAllEmployee(int pageNo,int pageSize,string sortOrder,bool descyn, DateTime dfrom,DateTime dto,int sex,string keyWord);

        /// <summary>Thêm mới nhân viên</summary>
        /// <param name="Employee">Đối tượng nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        bool AddEmployee(Employee Employee);

        /// <summary>Sửa nhân viên</summary>
        /// <param name="Employee">Đối tượng nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        bool SaveEditEmployee(Employee Employee);

        /// <summary>Xóa nhân viên</summary>
        /// <param name="idEmployee">Id nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        bool DeleteEmployee(Guid idEmployee);

        /// <summary>Kiểm tra sự tồn tại của Email</summary>
        /// <param name="email">Đầu vào email</param>
        /// <param name="idEmployee">Id nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        bool CheckExistsEmail(string email,Guid idEmployee);

        /// <summary>Kiểm tra sự tồn tại của SĐT</summary>
        /// <param name="phone">Đầu vào SĐT</param>
        /// <param name="idEmployee">ID của nhân viên</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        bool CheckExistsPhone(string phone, Guid idEmployee);

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
        /// lucnv 08-07-2022 created
        /// </Modified>
        int GetCountEmployee(DateTime dfrom, DateTime dto, int sex, string keyWord);
    }
}
