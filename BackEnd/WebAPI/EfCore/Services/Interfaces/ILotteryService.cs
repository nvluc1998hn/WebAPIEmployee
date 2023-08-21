﻿using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    public interface ILotteryService
    {
       bool AddLottery(Lottery lottery);

       bool UpdateLottery(Lottery lottery);

       bool DeleteLottery(Lottery lottery);

        List<Lottery> GetListData(LotteryRequest request);

        List<Lottery> GetListDataGroup(LotteryRequest request);
    }
}
