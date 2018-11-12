using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public class DocumentCrudRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly IEntityKeyAccossor _keyAccossor;
        private readonly object _lock = new object();

        public DocumentCrudRepository(IDocument<List<TEntity>> collection, IEntityKeyAccossor keyAccossor)
        {
            _keyAccossor = keyAccossor;
            Collection = collection;
        }

        protected IDocument<List<TEntity>> Collection { get; }

        public virtual IQueryable<TEntity> QueryRecords()
        {
            return Collection.Data.AsQueryable();
        }

        public virtual async Task<TEntity> AddOrUpdate(TEntity entity)
        {
            lock (_lock)
            {
                AddOrUpdateInternal(entity);
            }

            await Collection.Save();

            return entity;
        }

        public Task<TEntity> FindByKey(TKey key)
        {
            return Task.FromResult(Collection.Data.FirstOrDefault(FindByKeyPredicate(key)));
        }

        public async Task DeleteByKey(TKey key)
        {
            var item = await FindByKey(key);
            if (item == null)
            {
                throw new KeyNotFoundException("Can't find entity with key: " + key);
            }

            Collection.Data.Remove(item);
            await Collection.Save();
        }

        protected virtual void AddOrUpdateInternal(TEntity entity)
        {
            var key = _keyAccossor.GetKey<TEntity, TKey>(entity);
            if (key == null)
                throw new InvalidOperationException($"object key is missing");

            var item = Collection.Data.FirstOrDefault(FindByKeyPredicate(key));
            if (item != null)
            {
                var index = Collection.Data.IndexOf(item);
                Collection.Data.RemoveAt(index);
                Collection.Data.Insert(index, entity);
            }
            else
            {
                Collection.Data.Add(entity);
            }
        }

        private Func<TEntity, bool> FindByKeyPredicate(TKey key)
        {
            return entity =>
            {
                var entityKey = _keyAccossor.GetKey<TEntity, TKey>(entity);
                return EqualityComparer<TKey>.Default.Equals(entityKey, key);
            };
        }

        // public virtual async Task BatchAddOrUpdate(TEntity[] entities)
        // {
        //    lock (_lock)
        //     {
        //         foreach (var entity in entities)
        //             AddOrUpdateInternal(entity);
        //     }
        //     await Collection.Save();
        // }
        // public virtual async Task BatchDelete(Expression<Func<TEntity, bool>> predicate)
        // {
        //     var compiled = predicate.Compile();
        //     Collection.Data.RemoveAll(_ => compiled(_));
        //     await Collection.Save();
        // }
    }
}
