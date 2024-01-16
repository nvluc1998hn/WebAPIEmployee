using Base.Common.Interfaces;
using Base.Common.Models;
using EmployeeManagement.Common.Event;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Event;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{

    public class GridBaseService<TEntity, TId>  : IBaseService<TEntity, TId> where TEntity : class
    {
        protected readonly IRepositoryAsync<TEntity, TId> _repository;
        protected readonly ILogger<GridBaseService<TEntity, TId>> _logger;


        protected GridBaseService(IServiceProvider provider)
        {
            _repository = provider.GetService<IRepositoryAsync<TEntity, TId>>();
            _logger = provider.GetService<ILogger<GridBaseService<TEntity, TId>>>();
        }

        /// <summary> Xóa data từ DB </summary>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public virtual async Task<bool> DeleteAsync(TEntity data)
        {
            bool result;
            try
            {
                var entity = await _repository.DeleteAsync(data);
                result = entity != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi xóa data vào: {ex}");
                result = false;
            }
            return result;
        }

        /// <summary> Xóa 1 list theo điều kiện </summary>
        /// <param name="data"> </param>
        /// <param name="whereConditions"> </param>
        /// <param name="transaction"> </param>
        /// <param name="commandTimeout"> </param>
        /// <returns> </returns>
        public virtual async Task<bool> DeleteListAsync(TEntity data, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            bool result;
            try
            {
                var entity = await _repository.DeleteListAsync(data, whereConditions, transaction, commandTimeout);
                result = entity != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi xóa data vào: {ex}");
                result = false;
            }
            return result;
        }

        /// <summary> Xóa 1 list theo câu điều kiện </summary>
        /// <param name="data"> </param>
        /// <param name="conditions"> </param>
        /// <param name="parameters"> </param>
        /// <param name="transaction"> </param>
        /// <param name="commandTimeout"> </param>
        /// <returns> </returns>
        public virtual async Task<bool> DeleteListAsync(TEntity data, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            bool result;
            try
            {
                var entity = await _repository.DeleteListAsync(data, conditions, parameters, transaction, commandTimeout);
                result = entity != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi xóa data vào: {ex}");
                result = false;
            }
            return result;
        }

        /// <summary> Insert data vào DB </summary>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public virtual async Task<TEntity> InsertAsync(TEntity data)
        {
            TEntity result;
            try
            {
                result = await _repository.AddAsync(data);
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi Insert data vào: {ex}");
                result = null;
            }
            return result;
        }

        /// <summary> Insert 1 list data vào DB </summary>
        public virtual async Task<List<TEntity>> InsertListAsync(List<TEntity> listData)
        {
            List<TEntity> result;
            try
            {
                result = await _repository.AddAsync(listData);
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi Insert data vào: {ex}");
                result = null;
            }
            return result;
        }

        /// <summary> Update data vào DB </summary>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public virtual async Task<bool> UpdateAsync(TEntity data)
        {
            bool result;
            try
            {
                var entity = await _repository.UpdateAsync(data);
                result = entity != null;
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi update data vào: {ex}");
                result = false;
            }
            return result;
        }

        /// <summary> Update danh sách data vào DB </summary>
        /// <param name="listData"> </param>
        /// <returns> </returns>
        public virtual async Task<bool> UpdateListAsync(List<TEntity> listData)
        {
            bool result;
            try
            {
                var entity = await _repository.UpdateAsync(listData);
                result = entity != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi update data vào: {ex}");
                result = false;
            }
            return result;
        }


        /// <summary> Update dạng sql </summary>
        /// <param name="listData"> </param>
        /// <param name="conditions"> </param>
        /// <param name="parameters"> </param>
        /// <param name="transaction"> </param>
        /// <param name="commandTimeout"> </param>
        /// <returns> </returns>
        public virtual async Task<bool> UpdateListByConditionAsync(List<TEntity> listData, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            bool result;
            try
            {
                var entities = await _repository.UpdateListAsync(listData, conditions, parameters, transaction, commandTimeout);
                result = entities != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi khi cập nhật list data: {ex}");
                result = false;
            }
            return result;
        }
    }
}
