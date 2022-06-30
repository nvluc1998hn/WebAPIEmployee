using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Common.Constant
{
    public class ResultResponse
    {

        public dynamic Result { get; set; }
        public bool Success { get; set; }
        public string messenger { get; set; }
    }
}
