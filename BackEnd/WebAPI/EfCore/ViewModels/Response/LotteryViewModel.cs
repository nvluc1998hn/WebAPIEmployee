using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.ViewModels.Response
{
    public class LotteryViewModel : Lottery
    {
        // Giá lô
        public int PriceAmountLot { get; set; }
        // Thanh toán giá lô
        public int PaymentAmoutLot { get; set; }
        // Thanh toán giá đề
        public int PaymentAmountLopic { get; set; }  
    }
}
