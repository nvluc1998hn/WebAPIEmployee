using Autofac.Core;
using Base.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Azure;
using Base.Common.Controllers;
using Base.Common.Service.Interfaces;
using Base.Common.Constant;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    public class GridBaseCRUDController<TRequest, TRquestSearch, TResponse, IService> : GridBaseController<TRquestSearch, TResponse, IService>  where TRequest : BaseModel where IService: IBaseCRUDService<TRequest, TRquestSearch, TResponse, Guid>
    {
        public GridBaseCRUDController(IServiceProvider provider):base(provider)
        {

        }

        /// <summary> Thêm mới </summary>
        [HttpPost]
        public virtual async Task<ApiResponse> Insert([FromBody] TRequest request)
        {
            ApiResponse res;
            try
            {
                        request.CreatedDate = DateTime.Now;
                        var handleRes = await _service.Add(request);
                        if (handleRes.Success)
                        {
                            res = new ApiOkResultResponse(handleRes.Success, "Thêm mới thành công");
                        }
                        else
                        {
                            res = new ApiBadRequestResponse(handleRes.Data, handleRes.Message, handleRes.InternalMessage);
                        }
                   
            }
            catch (Exception ex)
            {
                res = new ApiCatchResponse(ex);
            }

            return res;
        }
        
      

        /// <summary> Sửa </summary>
        [HttpPut]
        public virtual async Task<ApiResponse> Update([FromBody] TRequest request)
        {
            ApiResponse res;

            try
            {

                var handleRes = await _service.Update(request);
                if (handleRes.Success)
                {
                    res = new ApiOkResultResponse(handleRes.Success, "Sửa thành công");
                }
                else
                {
                    res = new ApiBadRequestResponse(handleRes.Data, handleRes.Message, handleRes.InternalMessage);
                }

            }
            catch (Exception ex)
            {
                res = new ApiCatchResponse(ex);
            }

            return res;
        }

        /// <summary> Xóa </summary>
        [HttpDelete]
        public virtual async Task<ApiResponse> Delete([FromBody] TRequest request)
        {
            ApiResponse res;

            try
            {

                var handleRes = await _service.Delete(request);
                if (handleRes.Success)
                {
                    res = new ApiOkResultResponse(handleRes.Success, "Xóa thành công");
                }
                else
                {
                    res = new ApiBadRequestResponse(handleRes.Data, handleRes.Message, handleRes.InternalMessage);
                }

            }
            catch (Exception ex)
            {
                res = new ApiCatchResponse(ex);
            }

            return res;
        }

    }
}
