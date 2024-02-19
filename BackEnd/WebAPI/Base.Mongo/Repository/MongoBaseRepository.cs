using Base.Common.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base.Mongo.Repository
{
    public class MongoBaseRepository<TEntity, TypeId> : IMongoBaseRepository<TEntity, TypeId> where TEntity : BaseModel<TypeId>
    {
        protected readonly ILogger<MongoBaseRepository<TEntity, TypeId>> _logger;
        protected readonly IMongoDatabase _database;
        protected readonly IMongoCollection<TEntity> _collection;
        private string collectionName => typeof(TEntity).Name;

        public MongoBaseRepository(ILogger<MongoBaseRepository<TEntity, TypeId>> logger, IMongoDatabase database)
        {
            _logger = logger;
            _database = database;
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        /// <summary> Set thời gian tự xóa dữ liệu cho bảng </summary>
        /// <param name="minutes"> </param>
        /// <returns> </returns>
        public async Task SetDataExpireTimeFromCreatedDate(double minutes)
        {
            if (minutes > 0)
            {
                var createdDateProp = typeof(TEntity).GetProperty("CreatedDate");
                if (createdDateProp != null)
                {
                    var indexKeysDefinition = Builders<TEntity>.IndexKeys.Ascending(c => c.CreatedDate);
                    var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromMinutes(minutes) };
                    var indexModel = new CreateIndexModel<TEntity>(indexKeysDefinition, indexOptions);
                    await _collection.Indexes.CreateOneAsync(indexModel);
                }
            }
        }

        /// <summary> Lấy bản ghi theo id </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<TEntity> GetByIdAsync(TypeId id)
        {
            TEntity entity;
            try
            {
                entity = await _collection.Find(FilterId(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                entity = null;
            }
            return entity;
        }

        /// <summary> Lấy toàn bộ bản ghi </summary>
        /// <returns> </returns>
        public async Task<List<TEntity>> GetAll()
        {
            List<TEntity> entities;
            try
            {
                entities = await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                entities = null;
            }
            return entities;
        }

        /// <summary> Lấy danh sách bản ghi gần nhất theo CreatedDate và UpdatedDate </summary>
        /// <param name="date"> </param>
        /// <returns> </returns>
        public virtual async Task<List<TEntity>> GetLatestByCreatedOrUpdatedDate(DateTime date)
        {
            List<TEntity> entities;
            try
            {
                var filterCreatedDate = Builders<TEntity>.Filter.Gte(c => c.CreatedDate, date);
                var filterUpdatedDate = Builders<TEntity>.Filter.Gte(c => c.UpdatedDate, date);
                entities = await _collection.Find(filterCreatedDate | filterUpdatedDate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                entities = null;
            }
            return entities;
        }

        /// <summary> Lấy danh sách theo list Ids </summary>
        /// <param name="ids"> </param>
        /// <returns> </returns>
        public async Task<List<TEntity>> GetByListIds(IEnumerable<TypeId> ids)
        {
            List<TEntity> entities;
            try
            {
                var filter = Builders<TEntity>.Filter.In(c => c.Id, ids);
                entities = await _collection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                entities = null;
            }
            return entities;
        }

        public async Task<List<TEntity>> GetListByProperty<T>(IEnumerable<T> listPropvalues, string fieldName)
        {
            List<TEntity> entities;
            try
            {
                var filter = Builders<TEntity>.Filter.In(fieldName, listPropvalues);
                entities = await _collection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                entities = null;
            }
            return entities;
        }

        /// <summary> Lấy danh sách tìm kiếm giống lệnh like SQL </summary>
        /// <param name="ids"> </param>
        /// <returns> </returns>
        public async Task<List<TEntity>> GetLike(string search, string fieldName = "_id")
        {
            List<TEntity> entities;
            try
            {
                var key = Regex.Replace(search, @"\(", "\\(");
                key = Regex.Replace(key, @"\)", "\\)");

                var queryExpr = new BsonRegularExpression(new Regex(key, RegexOptions.IgnoreCase));

                var builders = Builders<TEntity>.Filter;
                var filter = builders.Regex(fieldName, queryExpr);

                entities = await _collection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                entities = null;
            }
            return entities;
        }

        /// <summary> Thêm 1 bản ghi </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        public async Task<bool> InsertAsync(TEntity entity)
        {
            bool result;
            try
            {
                await _collection.InsertOneAsync(entity);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary> Thêm danh sách, bỏ qua dòng bị trùng, vào exception nếu có dòng bị trùng </summary>
        /// <param name="entities"> </param>
        /// <returns> </returns>
        public async Task<bool> InsertListAsync(IEnumerable<TEntity> entities)
        {
            bool result;
            try
            {
                await _collection.InsertManyAsync(entities, new() { IsOrdered = false });
                result = true;
            }
            catch (Exception ex)
            {
                //Cắt ra do nếu để nguyên sẽ có nhiều dòng quá trong trường hợp ghi lại toàn bộ
                _logger.LogError(ex.Message.Substring(0, 500000));
                result = false;
            }
            return result;
        }

        /// <summary> Cập nhật, không thêm nếu không tồn tại </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            bool result;
            try
            {
                var res = await _collection.ReplaceOneAsync(FilterId(entity.Id), entity, new ReplaceOptions() { IsUpsert = false });
                result = res.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary> Cập nhật list, không thêm nếu không tồn tại </summary>
        /// <param name="entities"> </param>
        /// <returns> </returns>
        public async Task<bool> UpdateListAsync(IEnumerable<TEntity> entities)
        {
            bool result;
            try
            {
                var updates = entities.Select(entity =>
                {
                    var replaceModel = new ReplaceOneModel<TEntity>(FilterId(entity.Id), entity)
                    {
                        IsUpsert = false
                    };
                    return replaceModel;
                }).ToList();
                var res = await _collection.BulkWriteAsync(updates);
                result = res.ModifiedCount == entities.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary> Cập nhật, có thêm nếu không tồn tại </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        public async Task<bool> UpdateOrInsertAsync(TEntity entity)
        {
            bool result;
            try
            {
                await _collection.ReplaceOneAsync(FilterId(entity.Id), entity, new ReplaceOptions() { IsUpsert = true });
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary> Cập nhật list, có thêm nếu không tồn tại </summary>
        /// <param name="entities"> </param>
        /// <returns> </returns>
        public async Task<bool> UpdateOrInsertListAsync(IEnumerable<TEntity> entities)
        {
            bool result;
            try
            {
                var updates = entities.Select(entity =>
                {
                    var replaceModel = new ReplaceOneModel<TEntity>(FilterId(entity.Id), entity)
                    {
                        IsUpsert = true
                    };
                    return replaceModel;
                }).ToList();
                await _collection.BulkWriteAsync(updates);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary> Xóa bản ghi theo id </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<bool> DeleteByIdAsync(TypeId id)
        {
            bool result;
            try
            {
                await _collection.DeleteOneAsync(FilterId(id));
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        public async Task<bool> DeleteAsync(TEntity item) => await DeleteByIdAsync(item.Id);

        /// <summary> Xóa list theo id </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<bool> DeleteListByIdAsync(IEnumerable<TypeId> ids)
        {
            bool result;
            try
            {
                await _collection.DeleteManyAsync(Builders<TEntity>.Filter.In(c => c.Id, ids));
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary> Xóa danh sách </summary>
        /// <param name="entities"> </param>
        /// <returns> </returns>
        public async Task<bool> DeleteListAsync(IEnumerable<TEntity> entities) => await DeleteListByIdAsync(entities.Select(c => c.Id).ToList());

        /// <summary> Xóa danh sách theo property </summary>
        /// <param name="listPropValues"> Danh sách giá trị </param>
        /// <param name="fieldName"> Tên property </param>
        /// <returns> </returns>
        public async Task<bool> DeleteListByPropertyAsync<T>(IEnumerable<T> listPropValues, string fieldName)
        {
            bool result;
            try
            {
                await _collection.DeleteManyAsync(Builders<TEntity>.Filter.In(fieldName, listPropValues));
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary> Xóa tất cả </summary>
        /// <returns> </returns>
        public async Task<bool> DeleteAll()
        {
            bool result;
            try
            {
                await _database.DropCollectionAsync(collectionName);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }
            return result;
        }

        private static FilterDefinition<TEntity> FilterId(TypeId id)
        {
            return Builders<TEntity>.Filter.Eq(c => c.Id, id);
        }

    }
    
}
