using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Service.Interfaces
{
    public interface IBaseService<TEntity, TKey>  where TEntity : class
    {
        Task<TEntity> InsertAsync(TEntity data);

        Task<List<TEntity>> InsertListAsync(List<TEntity> listData);

        Task<bool> UpdateAsync(TEntity data);

        Task<bool> UpdateListAsync(List<TEntity> listData);

        Task<bool> UpdateListByConditionAsync(List<TEntity> listData, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<bool> DeleteAsync(TEntity data);

        Task<bool> DeleteListAsync(TEntity data, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<bool> DeleteListAsync(TEntity data, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

    }
}
