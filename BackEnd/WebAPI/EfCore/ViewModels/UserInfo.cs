using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.ViewModels
{
    public class UserInfo
    {
        public Guid EmployeeId { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }
     
    }
}
