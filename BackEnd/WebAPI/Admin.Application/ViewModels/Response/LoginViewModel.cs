using Admin.Application.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Admin.Application.ViewModels.Respond
{
    public class LoginViewModel
    {
        public LoginStatus Status { set; get; }

        public int CompanyId { set; get; }

        public int ParentCompanyID { get; set; }

        public Guid UserId { set; get; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; } 

        public string AccessToken { get; set; }

        public UserType UserType { get; set; }

        public string FullName { get; set; }

        public List<int> Permissions { set; get; }
    }
}
