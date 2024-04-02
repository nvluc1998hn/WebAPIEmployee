using Base.Common.Respone;
using Base.Common.Service.Interfaces;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.ViewModels.Request;
using EmployeeManagement.EfCore.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    /// <summary>
    ///  Dịch vụ - khách hàng
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 10/01/2024 created
    /// </Modified>
    public interface ICustomerServiceService: IBaseService<CustomerService2,string>, IGridBaseService<CustomerServiceRequest, CustomerServiceViewModel>
    {
        Task<ReturnType> InsertListCustomerService(List<CustomerService2> listDataInsert);

        Task<ReturnType> UpdateListCustomerService(List<CustomerService2> listDataInsert);

        Task<ReturnType> DeleteListCustomerService(List<CustomerService2> listDataInsert);

        Task<ReturnType> DeleteMulti(CustomerServiceDeleteRequest request);

        Task<ReturnType> DeletCustomerService(CustomerService2 customerService);


    }
}
