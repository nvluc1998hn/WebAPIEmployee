using Base.Common.Enum;
using Dapper;
using Base.Common.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Base.Common.Dapper;
using Base.Common.Dapper.SimpleCRUD;

namespace Base.Common.Implementations
{
    /// <summary>Thư viện dapper</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 26/10/2023 created
    /// </Modified>
    public class GenericRepository<TEntity, TId> : IRepositoryAsync<TEntity, TId> where TEntity : class
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private DatabaseNames _databaseNames = DatabaseNames.Default;

        public GenericRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        #region Method Not Async
        public TEntity GetById(TId id)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var entity = conn.Get<TEntity>(id);
            return entity;
        }

        public TEntity GetSingleByCondition(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var entity = conn.GetSingleByCondition<TEntity>(whereConditions, transaction, commandTimeout);
            return entity;
        }

        public IEnumerable<TEntity> GetList()
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var entities = conn.GetList<TEntity>();
            return entities;
        }

        public IEnumerable<TEntity> GetList(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.GetList<TEntity>(whereConditions, transaction, commandTimeout);
            return result;
        }

        public IEnumerable<TEntity> GetList(string conditions, string orderBy, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.GetList<TEntity>(conditions, orderBy, parameters, transaction, commandTimeout);
            return result;
        }



        public IEnumerable<T> QueryDapperStoreProc<T>(string store, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.QueryDapperStoreProc<T>(store, param);
            return result;
        }

        public int ExecuteDapperStoreProc<T>(string store, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.ExecuteDapperStoreProc(store, param);
            return result;
        }

        public IEnumerable<T> QueryDapper<T>(string sql, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.QueryDapper<T>(sql, param);
            return result;
        }

        public int ExecuteDapper<T>(string sql, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.ExecuteDapper(sql, param);
            return result;
        }

        public IEnumerable<T> QueryDapperStoreProcReport<T>(string store, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.QueryDapperStoreProc<T>(store, param);
            return result;
        }

        #endregion Not Async

        #region Method Async

        public async Task<IEnumerable<TEntity>> GetListAsync()
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.GetListAsync<TEntity>();
            return result;
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.GetListAsync<TEntity>(whereConditions, transaction, commandTimeout);
            return result;
        }

        public async Task<IEnumerable<TEntity>> GetTopListAsync(int number, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.GetTopListAsync<TEntity>(number, whereConditions, transaction, commandTimeout);
            return result;
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(string conditions, string orderBy, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.GetListAsync<TEntity>(conditions, orderBy, parameters, transaction, commandTimeout);
            return result;
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var entity = await conn.GetAsync<TEntity>(id);
            return entity;
        }

        public async Task<TEntity> GetSingleByConditionAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var entity = await conn.GetSingleByConditionAsync<TEntity>(whereConditions, transaction, commandTimeout);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var newId = await conn.InsertAsync<TId, TEntity>(entity);

            return entity;
        }

        public async Task<List<TEntity>> AddAsync(List<TEntity> entities)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var newIds = await conn.InsertListAsync<TId, TEntity>(entities);
            int index = 0;
            foreach (var entity in entities)
            {
                if (newIds.Count > 0)
                {
                    index++;
                }
            }

            return entities;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var numberRecordAffected = await conn.UpdateAsync(entity);
            if (numberRecordAffected <= 0)
            {
                throw new Exception($"Could not update record to the database. {entity}");
            }

            return entity;
        }

        public async Task<List<TEntity>> UpdateAsync(List<TEntity> entities)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            using var transaction = conn.BeginTransaction();
            var numberRecordAffected = await conn.UpdateAsync(entities, transaction);
            transaction.Commit();
            if (numberRecordAffected <= 0)
            {
                throw new Exception($"Could not update record to the database. {entities}");
            }

            return entities;
        }

        public async Task<TEntity> UpdateListAsync(TEntity entity, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);

            await conn.UpdateListAsync<TEntity>(conditions, parameters, transaction, commandTimeout);


            return entity;
        }

        public async Task<List<TEntity>> UpdateListAsync(List<TEntity> entities, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);

            await conn.UpdateListAsync<TEntity>(conditions, parameters, transaction, commandTimeout);


            return entities;
        }

        /// <summary>
        /// Cấm dùng hàm này
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> UpdateWithoutConditionAsync(TEntity entity)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var numberRecordAffected = await conn.UpdateWithoutConditionAsync(entity);
            if (numberRecordAffected <= 0)
            {
                throw new Exception($"Could not update record to the database. {entity}");
            }


            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            await conn.DeleteAsync(entity);

            return entity;
        }

        public async Task<TEntity> DeleteListAsync(TEntity entity, object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);

            await conn.DeleteListAsync<TEntity>(whereConditions, transaction, commandTimeout);

            return entity;
        }

        public async Task<TEntity> DeleteListAsync(TEntity entity, string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);

            await conn.DeleteListAsync<TEntity>(conditions, parameters, transaction, commandTimeout);

            return entity;
        }

        public async Task<int> RecordCountAsync(string conditions = "", object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.RecordCountAsync<TEntity>(conditions, parameters, transaction, commandTimeout);
            return result;
        }

        public async Task<int> RecordCountAsync(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.RecordCountAsync<TEntity>(whereConditions, transaction, commandTimeout);
            return result;
        }

        public async Task<IEnumerable<T>> QueryDapperStoreProcAsync<T>(string store, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.QueryDapperStoreProcAsync<T>(store, param);
            return result;
        }

        public async Task<int> ExecuteDapperStoreProcAsync<T>(string store, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.ExecuteDapperStoreProcAsync(store, param);
            return result;
        }

        public async Task<IEnumerable<T>> QueryDapperAsync<T>(string sql, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.QueryDapperAsync<T>(sql, param);
            return result;
        }

        public async Task<int> ExecuteDapperAsync<T>(string sql, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.ExecuteDapperAsync(sql, param);
            return result;
        }

        public Task<IEnumerable<T>> QueryDapperStoreProcReportAsync<T>(string store, object param = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = conn.QueryDapperStoreProcReportAsync<T>(store, param);
            return result;
        }

        /// <summary>
        /// Tương tự với GetListAsync, nhưng với trường hợp câu điều kiện có join với bảng khác
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="orderBy"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetListAsyncWithJoinConditions(string conditions, string orderBy, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection(_databaseNames);
            var result = await conn.GetListAsyncWithJoinConditions<TEntity>(conditions, orderBy, parameters, transaction, commandTimeout);
            return result;
        }

        #endregion Method Async
    }
}
