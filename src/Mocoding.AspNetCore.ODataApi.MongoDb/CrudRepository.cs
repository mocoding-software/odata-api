using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mocoding.AspNetCore.ODataApi.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    public class CrudRepository<TData> : ICrudRepository<TData>
        where TData : class, IEntity, new()
    {
        private readonly IMongoDatabase _database;
        private readonly string _tableName;

        public CrudRepository(string connectionString, string name = null)
        {
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(connection.DatabaseName);
            _tableName = string.IsNullOrEmpty(name) ? typeof(TData).Name : name;
            Collection.EnsureIndexes();
        }

        protected IMongoCollection<TData> Collection => _database.GetCollection<TData>(_tableName);

        public virtual IQueryable<TData> QueryRecords() => Collection.AsQueryable();

        public virtual async Task<TData> AddOrUpdate(TData entity)
        {
            if (!entity.Id.HasValue)
                entity.Id = Guid.NewGuid();

            await Collection.ReplaceOneAsync(new BsonDocument("_id", entity.Id), entity, new UpdateOptions() { IsUpsert = true });

            return entity;
        }

        public virtual Task Delete(Guid id) => Collection.DeleteOneAsync(new BsonDocument("_id", id));

        public virtual Task BatchAddOrUpdate(TData[] entities)
        {
            var model = entities.Select(_ =>
            {
                if (!_.Id.HasValue)
                    _.Id = Guid.NewGuid();
                return new ReplaceOneModel<TData>(new BsonDocument("_id", _.Id), _) { IsUpsert = true };
            });

            return Collection.BulkWriteAsync(model);
        }

        public virtual Task BatchDelete(Expression<Func<TData, bool>> predicate) => Collection.DeleteManyAsync(predicate);
    }
}