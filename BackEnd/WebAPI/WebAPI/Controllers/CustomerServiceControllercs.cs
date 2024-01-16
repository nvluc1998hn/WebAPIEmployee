using EmployeeManagement.Common.Constant;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using EmployeeManagement.EfCore.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/v1/customer-service")]
    public class CustomerServiceController : GridBaseController<CustomerServiceRequest, CustomerServiceViewModel, ICustomerServiceService>
    {
        public CustomerServiceController(IServiceProvider provider) : base(provider)
        {

        }

        /// <summary>Xóa nhiều </summary>
        /// <param name="vehicleId">The identifier.</param>
        /// <param name="companyId">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 23/04/2023 created
        /// </Modified>
        [HttpPost]
        [Route("delete-all")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<ApiResponse> DeleteMutiple([FromBody] CustomerServiceDeleteRequest rq)
        {
            try
            {
                
                var result = await _service.DeleteMulti(rq);
                if (result.Status)
                {
                    return new ApiOkResultResponse(result.Status, "Xóa thành công");
                }
                else
                {
                    return new ApiBadRequestResponse(result, "Xóa không thành công");
                }
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse("Lỗi", ex.Message);
            }
        }

        [HttpPost]
        [Route("update-list")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<ApiResponse> UpdateMutiple([FromBody] ListCustomerServiceRequest rq)
        {
            try
            {
                // Clear data cũ 

                if (rq.lsDataRequest?.Count > 0)
                {
                    var requestDelete = new CustomerServiceDeleteRequest() { CustomerId = rq.lsDataRequest[0].CustomerId,InvoiDate = rq.lsDataRequest[0].InvoiceDate };

                    var result = await _service.DeleteMulti(requestDelete);

                    if (result.Status)
                    {
                        await _service.InsertListCustomerService(rq.lsDataRequest);
                    }

                    if (result.Status)
                    {
                        return new ApiOkResultResponse(result.Status, "Cập nhật thành công");
                    }
                    else
                    {
                        return new ApiBadRequestResponse(result, "Cập nhật không thành công");
                    }
                }
                else
                {
                    return new ApiBadRequestResponse("Lỗi");

                }
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse("Lỗi", ex.Message);
            }
        }

        [HttpPost]
        [Route("insert-list")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<ApiResponse> InsertMutiple([FromBody] ListCustomerServiceRequest rq)
        {
            try
            {

                var result = await _service.InsertListCustomerService(rq.lsDataRequest);
                if (result.Status)
                {
                    return new ApiOkResultResponse(result.Status, "Thêm thành công");
                }
                else
                {
                    return new ApiBadRequestResponse(result, "Thêm không thành công");
                }
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse("Lỗi", ex.Message);
            }
        }
    }
}
