using AutoMapper;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.EmployeeRepository;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class LotteryService : ILotteryService
    {
        private readonly ILotteryRepository _lotteryRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LotteryService> _logger;

        public LotteryService(IConfiguration configuration, IMapper mapper, ILotteryRepository lotteryRepository, ApplicationDbContext applicationDbContext, ILogger<LotteryService> logger)
        {

            _configuration = configuration;
            _mapper = mapper;
            _lotteryRepository = lotteryRepository;
            _logger = logger;
        }

        public bool AddLottery(Lottery lottery)
        {
            bool isSuccess = false;
            try
            {
               var idInsert =  _lotteryRepository.Insert(lottery, nameof(Lottery.LotteryID));
                if (idInsert != null)
                {
                    isSuccess =true;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("AddEmployee: " + ex.Message);
                isSuccess = false;
            }
            return isSuccess;
        }


        public bool UpdateLottery(Lottery lottery)
        {
            bool isSuccess = false;
            try
            {
                var isUpdate = _lotteryRepository.Update(lottery);
                isSuccess = isUpdate;   
            }
            catch (Exception ex)
            {

                _logger.LogError("AddEmployee: " + ex.Message);
                isSuccess = false;
            }
            return isSuccess;
        }

        public List<Lottery> GetListData(LotteryRequest request)
        {
            var listData = _lotteryRepository.GetAll().ToList();
            try
            {
                request.Keyword = request.Keyword?.Trim();
                listData = listData.Where(x =>
                {
                    var ok = true;
                    if (!string.IsNullOrEmpty(request.Keyword))
                    {
                        ok = x.NumberLottery!.ToLower().Contains(request.Keyword.ToLower());
                    }
                    ok = x.CreatedDate.Date == request.FromDate.Date;
                    return ok;
                }).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError("GetListData: " + ex.Message);
              
            }
            return listData;
        }

    }
}
