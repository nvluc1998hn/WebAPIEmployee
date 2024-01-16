using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.ViewModels.Request
{
    public class CustomerServiceDeleteRequest
    {
       public Guid CustomerId { get; set; }

       public DateTime InvoiDate { get; set; }

            
    }
}
