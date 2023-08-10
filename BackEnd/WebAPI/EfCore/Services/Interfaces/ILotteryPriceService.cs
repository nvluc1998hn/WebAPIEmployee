using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    public interface ILotteryPriceService
    {
        public bool AddLottery(LotteryPrice lottery);

        public List<LotteryPrice> GetLotteryPrices(string lottetyPriceId);
    }
}
