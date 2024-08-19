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
using Base.Common.Grid;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    public abstract class GridBaseCRUDController<TRequest, TRquestSearch, TResponse, IService> : GridBaseController<TRquestSearch, TResponse, IService>  where IService: IBaseCRUDService<TRequest, TRquestSearch, TResponse, Guid>
    {
        protected abstract GridPermissions Permissions { get; }

        protected GridBaseCRUDController(IServiceProvider provider):base(provider)
        {

        }

        /// <summary> Thêm mới </summary>
        [HttpPost]
        public virtual async Task<ApiResponse> Insert([FromBody] TRequest request)
        {
            ApiResponse res;
            try
            {
                        ProcessRequest(request);
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
        
        public virtual TRequest ProcessRequest(TRequest request)
        {
            return request;
        }
      

        /// <summary> Sửa </summary>
        [HttpPut]
        public virtual async Task<ApiResponse> Update([FromBody] TRequest request)
        {
            ApiResponse res;

            try
            {
                ProcessRequest(request);
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
                ProcessRequest(request);
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
