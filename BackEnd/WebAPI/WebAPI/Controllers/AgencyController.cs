using EmployeeAPI.Controllers;
using EmployeeManagement.Common.Constant;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Implementations;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/v1/agency")]
    public class AgencyController : ControllerBase
    {
        private readonly IAgencyService _agencyService;
        private ILogger<AgencyController> _logger;
        private readonly IConfiguration _config;

        public AgencyController(ILogger<BaseController> logger, IConfiguration config, ILogger<AgencyController> logger2, IConfiguration configuration, IAgencyService agencyService)
        {
            _config = config;
            _agencyService = agencyService;
            _logger = logger2;
        }

        [HttpGet("get/{nameAgency}")]
        public ApiResponse GetData(string nameAgency)
        {
            ApiResponse res;
            try
            {
                var data = _agencyService.GetListData(nameAgency);
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
        public ApiResponse SaveAgency([FromBody] Agency data)
        {
            ApiResponse res;
            try
            {
                data.AgencyId = Guid.NewGuid();
                data.CreatedDate = DateTime.Now;
                var isSuccess = _agencyService.Add(data);
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

        //[HttpPost("update")]
        //public ApiResponse UpdateLottery([FromBody] Agency data)
        //{
        //    ApiResponse res;
        //    try
        //    {
        //        data.UpdatedDate = DateTime.Now;
        //        var isSuccess = _agencyService.UpdateAgency(data);
        //        if (isSuccess)
        //        {
        //            res = new ApiOkResultResponse(isSuccess);
        //        }
        //        else
        //        {
        //            res = new ApiBadRequestResponse("Có lỗi xảy ra");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res = new ApiBadRequestResponse(ex.Message);

        //    }

        //    return res;

        //}

    }
}
