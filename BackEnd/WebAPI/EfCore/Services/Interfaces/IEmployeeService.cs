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
        bool CheckLogin(string userName, string passWord);
        Employee GetEmployeeById(Guid idEmployee);
        IEnumerable<Employee> GetAllEmployee(int PageNo,int PageSize,string SortOrder,bool descyn, DateTime dfrom,DateTime dto,int sex,string keyWord);
        bool AddEmployee(Employee Employee);
        bool SaveEditEmployee(Employee Employee);
        bool DeleteEmployee(Guid idEmployee);
        bool CheckExistsEmail(string email,Guid idEmployee);
        bool CheckExistsPhone(string phone, Guid idEmployee);
        int GetCountEmployee();

    }
}
