﻿using AutoMapper;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.EmployeeRepository;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly ApplicationDbContext _db;


        public LotteryService(IConfiguration configuration, IMapper mapper, ILotteryRepository lotteryRepository, ILogger<LotteryService> logger, ApplicationDbContext db)
        {

            _configuration = configuration;
            _mapper = mapper;
            _lotteryRepository = lotteryRepository;
            _logger = logger;
            _db = db;
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
                    if (ok)
                    {
                        ok = x.CreatedDate.Date == request.FromDate.Date;
                    }
                    if (ok)
                    {
                        // Kiểm tra kiểu lô hay đề
                        ok = x.TypeLottery == request.TypeLottery;
                    }
                    return ok;
                }).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError("GetListData: " + ex.Message);
              
            }
            return listData;
        }

        public bool DeleteLottery(Lottery lottery)
        {
            bool isSuccess = false;
            try
            {
                _lotteryRepository.Remove(lottery.LotteryID);
                isSuccess = true;
            }
            catch (Exception ex)
            {

                _logger.LogError("AddEmployee: " + ex.Message);
                isSuccess = false;
            }
            return isSuccess;
        }

        public List<Lottery> GetListDataGroup(LotteryRequest request)
        {
            var paramDate = new SqlParameter("Date", request.FromDate);
            var paramTypeLottery = new SqlParameter("TypeLottery", request.TypeLottery);

           var dataResult =  _db.Lotterys.FromSqlRaw("EXEC GetDataLotteryGroup @Date,@TypeLottery",
                         paramDate, paramTypeLottery);
            throw new NotImplementedException();
        }
    }
}
