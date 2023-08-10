using AutoMapper;
using EmployeeAPI.Controllers;
using EmployeeManagement.Common.Constant;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Implementations;
using EmployeeManagement.EfCore.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/v1/lottery-price")]
    public class LotteryPriceController : Controller
    {
        private readonly ILotteryPriceService _lotteryPriceService;
        private readonly ILogger<LotteryPriceController> _logger;

        public LotteryPriceController(ILogger<LotteryPriceController> logger, ILotteryPriceService lotteryPriceService)
        {
            _logger = logger;
            _lotteryPriceService = lotteryPriceService;
        }

        [HttpPost("insert")]
        public async Task<ApiResponse> SaveLotteryPrice([FromBody] LotteryPrice data)
        {
            ApiResponse res;
            try
            {
                data.LotteryPriceID = Guid.NewGuid();
                data.CreatedDate = DateTime.Now;
                var result = _lotteryPriceService.AddLottery(data);
                if (result)
                {
                    res = new ApiOkResultResponse(result);
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

        [HttpGet("get/{brandId}")]
        public async Task<ApiResponse> GetLotteryPrice(string brandId)
        {
            ApiResponse res;

            try
            {
                var data = _lotteryPriceService.GetLotteryPrices(brandId);
                if (data?.Count > 0)
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
    }
}
