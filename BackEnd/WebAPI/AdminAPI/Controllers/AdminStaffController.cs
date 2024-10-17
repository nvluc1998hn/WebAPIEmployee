using Admin.Application.Interfaces;
using Base.Common.Constant;
using Base.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Admin.API.Controllers
{
    [Route("admin-staff")]
    [ApiController]
    public class AdminStaffController : ApiController
    {
        private readonly IAdminStaffService _adminStaffService;

        public AdminStaffController(IAdminStaffService adminStaffService)
        {
            _adminStaffService = adminStaffService;
        }

        [HttpPost]
        [Route("getlist")]
        [ApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ApiResponse> GetListStaffByCondition(string keyword)
        {
            ApiResponse response;
            try
            {
                var result = await _adminStaffService.GetListStaffByCondition(keyword);

                response = new ApiOkResultResponse(result);
            }
            catch (Exception ex)
            {
                response = new ApiCatchResponse(ex);
            }

            return response;
        }
    }
}
