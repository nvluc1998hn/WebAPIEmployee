using AutoMapper;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class LotteryPriceService : ILotteryPriceService
    {
        private readonly ILotteryPriceRepository _lotteryPriceRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LotteryPriceService> _logger;

        public LotteryPriceService(IConfiguration configuration, ILotteryPriceRepository lotteryPriceRepository, ILogger<LotteryPriceService> logger)
        {

            _configuration = configuration;
            _lotteryPriceRepository = lotteryPriceRepository;
            _logger = logger;
        }

        public bool AddLottery(LotteryPrice lottery)
        {
            bool isSuccess = false;
            try
            {
                var idInsert = _lotteryPriceRepository.Insert(lottery, nameof(lottery.LotteryPriceID));
                if (idInsert != null)
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("AddEmployee: " + ex.Message);
                isSuccess = false;
            }
            return isSuccess;
        }

        public List<LotteryPrice> GetLotteryPrices(string lottetyPriceId)
        {
            List<LotteryPrice> listData = new List<LotteryPrice>();
            try
            {
                listData = _lotteryPriceRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError("AddEmployee: " + ex.Message);
            }
            return listData;
        }
    }
}
