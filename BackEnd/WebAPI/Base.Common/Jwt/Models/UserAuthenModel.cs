using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Models
{
    public class UserAuthenModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public int UserType { get; set; }

        public int CompanyId { get; set; }

        public int CompanyType { get; set; }

        // Danh sách quyền của user
        public List<int> Permissions { get; set; }
    }
}
