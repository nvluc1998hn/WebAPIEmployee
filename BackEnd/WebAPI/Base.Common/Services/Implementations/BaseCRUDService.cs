using AutoMapper;
using Base.Common.Cache.Redis.Interface;
using Base.Common.Event;
using Base.Common.Helper;
using Base.Common.Interfaces;
using Base.Common.Models;
using Base.Common.Service.Interfaces;
using Base.Domain.Models.EntityBase;
using Base.Mongo.Repository;
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
    /// Base thêm sửa xóa
    /// </summary>
    /// <typeparam name="TRequest">Entity gốc</typeparam>
    /// <typeparam name="TRquestSearch">Entity search</typeparam>
    /// <typeparam name="TResponse">Kết quả trả về</typeparam>
    /// <typeparam name="Id">Kiểu dữ liệu của Id</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 05/03/2024 created
    /// </Modified>
    public class BaseCRUDService<TRequest, TRquestSearch, TResponse, Id> : IBaseCRUDService<TRequest, TRquestSearch, TResponse, Id> where TRequest : BaseModel<Id>
    {
        private readonly   IMapper _mapper;
        protected readonly IRepositoryAsync<TRequest, Id> _repository;
        protected readonly IServiceCache _serviceCache;
        protected readonly IMongoBaseRepository<TRequest, Id> _mongoRepository;
        protected readonly ILogger<BaseCRUDService<TRequest, TRquestSearch, TResponse, Id>> _logger;

        public BaseCRUDService(IServiceProvider provider)
        {

            _mapper = provider.GetService<IMapper>();
            _mongoRepository = provider.GetService<IMongoBaseRepository<TRequest, Id>>();
            _serviceCache = provider.GetService<IServiceCache>();
            _repository = provider.GetService<IRepositoryAsync<TRequest, Id>>();
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
                    await _mongoRepository.InsertAsync(data);
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
                if (result.Success)
                {
                    result.Data = dataUpdate;
                    await _mongoRepository.UpdateAsync(data);

                }
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
            result.Success = false;
            try
            {
 
                var dataDelete = await _repository.DeleteAsync(data);
                result.Success = dataDelete != null;
                if (result.Success)
                {
                    result.Data = result;
                    await _mongoRepository.DeleteAsync(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi {MethodHelper.GetNameAsync()}: {ex}");
            }
            return result;
        }

        /// <summary> Biến đổi dữ liệu từ DB sang model dữ liệu trả về client </summary>
        /// <param name="dbItems"> Dữ liệu lấy ra từ DB theo điều kiện lọc của SetConditions </param>
        /// <param name="request"> Request </param>
        protected virtual async Task<IEnumerable<TResponse>> MapDBData(IEnumerable<TRequest> dbItems)
        {
            await Task.Delay(0);
            return _mapper.Map<IEnumerable<TResponse>>(dbItems);
        }

        /// <summary>
        /// Lấy dữ liệu grid phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 05/03/2024 created
        /// </Modified>
        public virtual Task<HandleResult<GridBaseResponse<TResponse>>> GetPage(TRquestSearch request)
        {
            throw new NotImplementedException();
        }
    }
}
