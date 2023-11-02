using EmployeeManagement.Common.Constant;
using EmployeeManagement.Database.Context.Models;
using Microsoft.AspNetCore.Mvc;
using VehicleManagement.EfCore.Services.Interfaces;

namespace VehicleManagementAPI.Controllers
{
    [ApiController]
    [Route("vehicle")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(ILogger<VehicleController> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        [HttpPost("insert")]
        public async Task<ApiResponse> SaveLotteryPrice([FromBody] LotteryPrice data)
        {
            ApiResponse res;
            try
            {
               
                res = new ApiOkResultResponse();
                
            }
            catch (Exception ex)
            {
                res = new ApiBadRequestResponse(ex.Message);

            }
            return res;

        }
    }
}
