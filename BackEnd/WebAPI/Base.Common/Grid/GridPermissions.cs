using Base.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Grid
{
    public class GridPermissions
    {
        public PermissionKeyNames View { get; set; }
        public PermissionKeyNames? Insert { get; set; }
        public PermissionKeyNames? Update { get; set; }
        public PermissionKeyNames? Delete { get; set; }
        public PermissionKeyNames? Export { get; set; }
    }
}
