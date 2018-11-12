using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    class CrudRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey>
        where TEntity : class, new()
    {
        private readonly IMongoDatabase _database;
        private readonly IEntityKeyAccossor _keyAccossor;
        private readonly string _tableName;

        public CrudRepository(IMongoDatabase database, IEntityKeyAccossor keyAccossor)
        {
            _database = database;
            _keyAccossor = keyAccossor;
            _tableName = typeof(TEntity).Name;
            Collection.EnsureIndexes();
        }

        protected IMongoCollection<TEntity> Collection => _database.GetCollection<TEntity>(_tableName);

        public virtual IQueryable<TEntity> QueryRecords() => Collection.AsQueryable();

        public virtual async Task<TEntity> AddOrUpdate(TEntity entity)
        {
            var id = _keyAccossor.GetKey<TEntity, TKey>(entity);
            if (id == null)
                throw new InvalidOperationException($"object id is missing");
            await Collection.ReplaceOneAsync(GetBsonDocument(id), entity, new UpdateOptions() {IsUpsert = true});
            return entity;
        }

        public async Task<TEntity> FindByKey(TKey key)
        {
            var result =  await Collection.FindAsync(GetBsonDocument(key));
            return result.First();
        }

        public Task DeleteByKey(TKey key)
        {
            return Collection.DeleteOneAsync(GetBsonDocument(key));
        }

        private BsonDocument GetBsonDocument(TKey key)
        {
            var bsonValue = BsonValue.Create(key);
            return new BsonDocument("_id", bsonValue);
        }

        //public virtual Task BatchAddOrUpdate(TEntity[] entities)
        //{
        //    var model = entities.Select(_ =>
        //    {
        //        if (!_.Id.HasValue)
        //            _.Id = Guid.NewGuid();
        //        return new ReplaceOneModel<TEntity>(new BsonDocument("_id", _.Id), _) { IsUpsert = true };
        //    });

        //    return Collection.BulkWriteAsync(model);
        //}

        //public virtual Task BatchDelete(Expression<Func<TEntity, bool>> predicate) => Collection.DeleteManyAsync(predicate);
    }
}