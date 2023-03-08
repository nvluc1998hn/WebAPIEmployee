using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Dapper.Repositories.Interfaces
{
    public interface IRepositoryAsync<TEntity, TId> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<List<TEntity>> AddAsync(List<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<List<TEntity>> UpdateAsync(List<TEntity> entities);

        Task<TEntity> UpdateListAsync(TEntity entity, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<List<TEntity>> UpdateListAsync(List<TEntity> entities, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<TEntity> UpdateWithoutConditionAsync(TEntity entity);

        Task<TEntity> DeleteAsync(TEntity entity);

        Task<TEntity> DeleteListAsync(TEntity entity, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<TEntity> DeleteListAsync(TEntity entity, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        int ExecuteDapperStoreProc<T>(string store, object param = null);

        int ExecuteDapper<T>(string sql, object param = null);

        Task<int> ExecuteDapperAsync<T>(string sql, object param = null);

        Task<int> ExecuteDapperStoreProcAsync<T>(string store, object param = null);
    }
}
