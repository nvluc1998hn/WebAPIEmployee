using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Interfaces
{

    /// <summary>Thư viện dapper</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 26/10/2023 created
    /// </Modified>
    public interface IRepositoryAsync<TEntity, TId> where TEntity : class
    {
        TEntity GetById(TId id);

        TEntity GetSingleByCondition(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        IEnumerable<TEntity> GetList();

        IEnumerable<TEntity> GetList(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        IEnumerable<TEntity> GetList(string conditions, string orderBy, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);


        IEnumerable<T> QueryDapperStoreProc<T>(string store, object param = null);

        IEnumerable<T> QueryDapper<T>(string sql, object param = null);

        IEnumerable<T> QueryDapperStoreProcReport<T>(string store, object param = null);

        Task<IEnumerable<TEntity>> GetListAsync();

        Task<IEnumerable<TEntity>> GetListAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IEnumerable<TEntity>> GetTopListAsync(int number, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IEnumerable<TEntity>> GetListAsync(string conditions, string orderBy, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<TEntity> GetByIdAsync(TId id);

        Task<TEntity> GetSingleByConditionAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> RecordCountAsync(string conditions = "", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> RecordCountAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IEnumerable<T>> QueryDapperStoreProcAsync<T>(string store, object param = null);

        Task<IEnumerable<T>> QueryDapperAsync<T>(string sql, object param = null);

        Task<IEnumerable<T>> QueryDapperStoreProcReportAsync<T>(string store, object param = null);

        Task<IEnumerable<TEntity>> GetListAsyncWithJoinConditions(string conditions, string orderBy, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddAsync(List<TEntity> entities);
    }
}
