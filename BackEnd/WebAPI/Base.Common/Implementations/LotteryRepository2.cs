using Base.Common.Dapper;
using Base.Common.Enum;
using Base.Common.Interfaces;
using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Implementations
{
    public class LotteryRepository2 : GenericRepository<Lottery, Guid>, ILotteryRepository2
    {
        public LotteryRepository2(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }
    }
}
