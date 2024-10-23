using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Base.Common.Constant;
using Base.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
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


        [HttpPost]
        [Route("insert")]
        [ApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<ApiResponse> InsertData(AdminStaffRequest dataInsert)
        {
            ApiResponse response;
            try
            {
                if (dataInsert.IsValid())
                {
                    var result = await _adminStaffService.InsertData(dataInsert);

                    return new ApiOkResultResponse(true);
                }
                else
                {
                    response = new ApiOkResultResponse(false, dataInsert.ValidationResultMessage);
                }
            }
            catch (Exception ex)
            {
                response = new ApiCatchResponse(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("update")]
        [ApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<ApiResponse> UpdateData(AdminStaffRequest dataUpdate)
        {
            ApiResponse response;
            try
            {
                if (dataUpdate.IsValid())
                {
                    var result = await _adminStaffService.UpdateData(dataUpdate);

                    return new ApiOkResultResponse(result);
                }
                else
                {
                    response = new ApiOkResultResponse(false, dataUpdate.ValidationResultMessage);
                }

            }
            catch (Exception ex)
            {
                response = new ApiCatchResponse(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("staffId/{id}")]
        [ApiVersion("1")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse response;
            try
            {
                var result = await _adminStaffService.DeleteData(id);

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
