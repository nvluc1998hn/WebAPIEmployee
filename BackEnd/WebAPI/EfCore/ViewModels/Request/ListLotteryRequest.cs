using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.ViewModels.Request
{
    public class ListLotteryRequest
    {
        public List<Lottery> lotteries { get; set; }
    }
}
