using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeManagementAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/v1/customer")]
    [ApiVersion("1")]
    public class CustomerController : GridBaseCRUDController<Customer, CustomerRequestSearch, Customer, ICustomerService>
    {
        public CustomerController(IServiceProvider provider) : base(provider)
        {

        }
    }
}
