using EmployeeAPI.Controllers;
using EmployeeManagement.Common.Constant;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Command.ActionCommand;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Serilog.Core;
using Serilog;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/v1/lottery")]
    public class LotteryController : BaseController
    {
        private readonly ILotteryService _lotteryService;
        private ILogger<LotteryController> _logger;

        public LotteryController(ILogger<BaseController> logger, ILogger<LotteryController> logger2, IConfiguration configuration, ILotteryService lotteryService) : base(logger, configuration)
        {
            _lotteryService = lotteryService;
            _logger = logger2;
        }

        [HttpPost("get")]
        public ApiResponse GetData([FromBody] LotteryRequest request)
        {
            ApiResponse res;
            
            try
            {
                var data = _lotteryService.GetListData(request);
                if(data?.Count > 0)
                {
                    res = new ApiOkResultResponse(data);

                }
                else
                {
                    res = new ApiNoContentResponse("Không có dữ liệu");

                }

            }
            catch (Exception ex)
            {
                res = new ApiBadRequestResponse("Có lỗi tại hàm Get Lottery");
                _logger.LogError(ex.ToString());

            }
            return res;
        }

        [HttpPost("insert")]
        public ApiResponse SaveLottery([FromBody] Lottery data)
        {
            ApiResponse res;
            try
            {
                data.LotteryID = Guid.NewGuid();
                data.CreatedDate = DateTime.Now;
                var isSuccess = _lotteryService.AddLottery(data);
                if (isSuccess)
                {
                    res = new ApiOkResultResponse(isSuccess);
                }
                else
                {
                    res= new ApiBadRequestResponse("Có lỗi xảy ra");

                }
            }
            catch (Exception ex)
            {
                res= new ApiBadRequestResponse(ex.Message);

            }

            return res;

        }

        [HttpPost("update")]
        public ApiResponse UpdateLottery([FromBody] Lottery data)
        {
            ApiResponse res;
            try
            {
                data.UpdatedDate = DateTime.Now;
                var isSuccess = _lotteryService.UpdateLottery(data);
                if (isSuccess)
                {
                    res = new ApiOkResultResponse(isSuccess);
                }
                else
                {
                    res = new ApiBadRequestResponse("Có lỗi xảy ra");

                }
            }
            catch (Exception ex)
            {
                res = new ApiBadRequestResponse(ex.Message);

            }

            return res;

        }

        [HttpPost("delete")]
        public ApiResponse DeleteLottery([FromBody] Lottery data)
        {
            ApiResponse res;
            try
            {
                var isSuccess = _lotteryService.DeleteLottery(data);
                if (isSuccess)
                {
                    res = new ApiOkResultResponse(isSuccess);
                }
                else
                {
                    res = new ApiBadRequestResponse("Có lỗi xảy ra");

                }
            }
            catch (Exception ex)
            {
                res = new ApiBadRequestResponse(ex.Message);

            }

            return res;

        }

    }
}
