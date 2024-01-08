using Base.Common.Interfaces;
using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Interfaces
{
    /// <summary>
    /// Danh sách khách hàng
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 03/01/2024 created
    /// </Modified>
    public interface ICustomerRepository: IRepositoryAsync<Customer,Guid>
    {

    }
}
