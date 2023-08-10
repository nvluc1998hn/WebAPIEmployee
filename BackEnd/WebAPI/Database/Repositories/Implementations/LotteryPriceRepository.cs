using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Implementations
{
    public class LotteryPriceRepository : Repository<LotteryPrice, Guid>, ILotteryPriceRepository
    {
        public LotteryPriceRepository(ApplicationDbContext context, ILogger<LotteryPrice> logger) : base(context, logger)
        {

        }
    }
}
