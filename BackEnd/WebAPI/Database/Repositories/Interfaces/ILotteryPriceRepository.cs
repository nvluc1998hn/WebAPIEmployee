using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Interfaces
{
    public interface ILotteryPriceRepository : IRepository<LotteryPrice, Guid>
    {

    }
}
