using Base.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Mongo.Repository
{
    public interface IMongoBaseRepository<TEntity, TypeId> where TEntity : BaseModel<TypeId>
    {
        Task SetDataExpireTimeFromCreatedDate(double minutes);

        Task<TEntity> GetByIdAsync(TypeId id);

        Task<List<TEntity>> GetAll();

        Task<List<TEntity>> GetLatestByCreatedOrUpdatedDate(DateTime date);

        Task<List<TEntity>> GetByListIds(IEnumerable<TypeId> ids);

        Task<List<TEntity>> GetListByProperty<T>(IEnumerable<T> listPropvalues, string fieldName);

        Task<List<TEntity>> GetLike(string search, string fieldName = "_id");

        Task<bool> InsertAsync(TEntity entity);

        Task<bool> InsertListAsync(IEnumerable<TEntity> entities);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> UpdateListAsync(IEnumerable<TEntity> entities);

        Task<bool> UpdateOrInsertAsync(TEntity entity);

        Task<bool> UpdateOrInsertListAsync(IEnumerable<TEntity> entities);

        Task<bool> DeleteByIdAsync(TypeId id);

        Task<bool> DeleteAsync(TEntity item);

        Task<bool> DeleteListByIdAsync(IEnumerable<TypeId> ids);

        Task<bool> DeleteListAsync(IEnumerable<TEntity> entities);

        Task<bool> DeleteListByPropertyAsync<T>(IEnumerable<T> listPropValues, string fieldName);

        Task<bool> DeleteAll();
    }
}
