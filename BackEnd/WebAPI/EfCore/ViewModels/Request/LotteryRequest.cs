using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.ViewModels.Request
{
    public class LotteryRequest
    {
        public DateTime FromDate { get;set; }

        public string Keyword { get; set; }
    }
}
