using EmployeeManagement.Database.Dapper.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Dapper.Repositories.Implementations
{
    public class GenericRepository<TEntity, TId> : IRepositoryAsync<TEntity, TId> where TEntity : class
    {
        public Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> AddAsync(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> DeleteListAsync(TEntity entity, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> DeleteListAsync(TEntity entity, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public int ExecuteDapper<T>(string sql, object param = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteDapperAsync<T>(string sql, object param = null)
        {
            throw new NotImplementedException();
        }

        public int ExecuteDapperStoreProc<T>(string store, object param = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteDapperStoreProcAsync<T>(string store, object param = null)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> UpdateAsync(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateListAsync(TEntity entity, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> UpdateListAsync(List<TEntity> entities, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateWithoutConditionAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
