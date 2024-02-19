using AutoMapper;
using Base.Common.Cache.Redis.Interface;
using Base.Common.Event;
using Base.Common.Helper;
using Base.Common.Interfaces;
using Base.Common.Models;
using Base.Common.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Base.Common.Services.Implementations
{
    /// <summary>
    /// CRUD
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <typeparam name="Id">The type of the d.</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 07/01/2024 created
    /// </Modified>
    public class BaseCRUDService<TRequest, TRquestSearch, TResponse, Id> : IBaseCRUDService<TRequest, TRquestSearch, TResponse, Id> 
    {
        private readonly IMapper _mapper;
        protected readonly IRepositoryAsync<TRequest, Id> _repository;
        protected readonly IServiceCache _serviceCache;
        protected readonly ILogger<BaseCRUDService<TRequest, TRquestSearch, TResponse, Id>> _logger;

        public BaseCRUDService(IServiceProvider provider)
        {

            _mapper = provider.GetService<IMapper>();
            _repository = provider.GetService<IRepositoryAsync<TRequest, Id>>();
            _serviceCache = provider.GetService<IServiceCache>();
            _logger = provider.GetService<ILogger<BaseCRUDService<TRequest, TRquestSearch, TResponse, Id>>>();
        }

        public async Task<HandleResult> Add(TRequest data)
        {
            var result = new HandleResult();
            result.Success = false;
            try
            {
                
                var dataInsert = await _repository.AddAsync(data);
                if (dataInsert != null)
                {
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi {MethodHelper.GetNameAsync()}: {ex}");
            }
            return result;
        }


        public async Task<HandleResult> Update(TRequest data)
        {
            var result = new HandleResult();
            result.Success = false;
            try
            {
                var dataUpdate = await _repository.UpdateAsync(data);
                result.Success = dataUpdate != null;
                result.Data = dataUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi {MethodHelper.GetNameAsync()}: {ex}");
            }
            return result;
        }

        public async Task<HandleResult> Delete(TRequest data)
        {
            var result = new HandleResult();
            result.Success = false; try
            {
 
                var dataDelete = await _repository.DeleteAsync(data);
                result.Success = dataDelete != null;
                result.Data = result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi {MethodHelper.GetNameAsync()}: {ex}");
            }
            return result;
        }

        public virtual Task<HandleResult<GridBaseResponse<TResponse>>> GetPage(TRquestSearch request)
        {
            throw new NotImplementedException();
        }
    }
}
