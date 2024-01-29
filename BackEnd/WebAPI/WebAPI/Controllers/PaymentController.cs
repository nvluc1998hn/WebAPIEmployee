using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/v1/payment")]
    [ApiVersion("1")]
    public class PaymentController : GridBaseCRUDController<Payment, PaymentRequestSearch, Payment, IPaymentService>
    {
        public PaymentController(IServiceProvider provider) : base(provider)
        {

        }
    }
}
