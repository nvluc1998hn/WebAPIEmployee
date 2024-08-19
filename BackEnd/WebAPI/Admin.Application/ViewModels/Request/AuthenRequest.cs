using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.ViewModels.Request
{
    public class AuthenRequest
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid LoginUserId { get; set; }
        public Guid SessionKey { get; set; }
        public string Token { get; set; }
    }
}
