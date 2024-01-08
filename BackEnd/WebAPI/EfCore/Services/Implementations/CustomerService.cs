using AutoMapper;
using Base.Common.Interfaces;
using EmployeeManagement.Common.Event;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Event;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class CustomerService : BaseCRUDService<Customer,CustomerRequestSearch, Customer, Guid>, ICustomerService
    {
        public CustomerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public override async Task<HandleResult<GridBaseResponse<Customer>>> GetPage(CustomerRequestSearch request)
        {
            var res = new HandleResult<GridBaseResponse<Customer>>() { Data = new(), Message = "Lấy dữ liệu không thành công"};

            try
            {

            }
            catch (Exception)
            {

                throw;
            }

            return res;
        }
    }
}
