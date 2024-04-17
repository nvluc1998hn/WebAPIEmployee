using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.ViewModels.Response
{
    public class AdminUserViewModel
    {
        public Guid PK_UserID { get; set; }

        public int FK_CompanyID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }
    }
}
