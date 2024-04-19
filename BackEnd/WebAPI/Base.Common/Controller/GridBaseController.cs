using Autofac.Core;
using AutoMapper;
using Base.Common.Cache.Redis.Interface;
using Base.Common.Constant;
using Base.Common.Models;
using Base.Common.Service.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Base.Common.Controllers
{

    [ApiController]
    public class GridBaseController<TRquestSearch, TResponse, IService> : ApiController  where IService : IGridBaseService<TRquestSearch, TResponse>
    {
        protected readonly ILogger<GridBaseController<TRquestSearch, TResponse, IService>> _logger;
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly IService _service;
        protected readonly IServiceCache _serviceCache;

        public GridBaseController(IServiceProvider provider)
        {
            _logger = provider.GetService<ILogger<GridBaseController<TRquestSearch, TResponse, IService>>>();
            _mediator = provider.GetService<IMediator>();
            _mapper = provider.GetService<IMapper>();
            _service = provider.GetService<IService>();
            _serviceCache = provider.GetService<IServiceCache>();
        }

        /// <summary> Lấy dữ liệu phân trang </summary>
        [HttpPost("page")]
        public virtual async Task<ApiResponse> GetPage([FromBody] TRquestSearch request)
        {
            ApiResponse res;

            try
            {
                var respone = await _service.GetPage(request);

                if (respone.Success || respone.Data?.Items?.Count > 0)
                {
                    if (respone.Data.Items?.Count > 0)
                    {
                        respone.Data.TotalItems = respone.Data.Items.Count;

                        res = new ApiOkResultResponse(respone.Data, "Lấy dữ liệu thành công", respone.InternalMessage ?? "Lấy dữ liệu thành công");
                    }
                    else
                    {
                        res = new ApiNoDataResponse(null, respone.InternalMessage);
                    }
                }
                else
                {
                    res = new ApiBadRequestResponse(respone.Message, respone.InternalMessage);
                }
            }
            catch (Exception ex)
            {
                res = new ApiCatchResponse(ex);
                _logger.LogError($"Lỗi {GetType().Name}: {ex}");
            }

            return res;
        }
    }
}
