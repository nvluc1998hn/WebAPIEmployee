using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class PaymentService : BaseCRUDService<Payment, PaymentRequestSearch, Payment, Guid>, IPaymentService
    {
        public PaymentService(IServiceProvider provider) : base(provider)
        {

        }
    }
}
