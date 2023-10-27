using EmployeeAPI.Controllers;
using EmployeeManagement.Common.Constant;
using EmployeeManagement.Database.Context.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Grpc.Net.Client;
using GrpcService.Protos;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/v1/lottery")]
    public class LotteryController : ControllerBase
    {
        private readonly ILotteryService _lotteryService;
        private ILogger<LotteryController> _logger;
        private readonly IConfiguration _config;

        public LotteryController(ILogger<BaseController> logger, IConfiguration config, ILogger<LotteryController> logger2, IConfiguration configuration, ILotteryService lotteryService) 
        {
            _config = config;
            _lotteryService = lotteryService;
            _logger = logger2;
        }

        [HttpPost("get")]
        public async Task<ApiResponse> GetData([FromBody] LotteryRequest request)
        {
            ApiResponse res;       
            try
            {
                var channel = GrpcChannel.ForAddress("http://localhost:5080");
                var client = new Product.ProductClient(channel);
                var product = new GetProductDetail
                {
                    ProductId = 3
                };
                var serverReply = await client.GetProductsInformationAsync(product);

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

        [HttpPost("get-data-group")]
        public ApiResponse GetDataGroup([FromBody] LotteryRequest request)
        {
            ApiResponse res;

            try
            {
                var data = _lotteryService.GetListDataGroup(request);
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

        [HttpPost("insert-list")]
        public ApiResponse SaveListLottery([FromBody] ListLotteryRequest data)
        {
            ApiResponse res;
            try
            {
                var isSuccess = _lotteryService.AddListLottery(data.lotteries);
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
