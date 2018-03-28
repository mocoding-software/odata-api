using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.AspNetCore.ODataApi.DataAccess;
using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public class DocumentCrudRepository<TData> : ICrudRepository<TData>
        where TData : class, IEntity, new()
    {
        private readonly object _lock = new object();

        public DocumentCrudRepository(IDocument<List<TData>> collection)
        {
            Collection = collection;
        }

        protected IDocument<List<TData>> Collection { get; }

        public virtual IQueryable<TData> QueryRecords()
        {
            return Collection.Data.AsQueryable();
        }

        public virtual async Task<TData> AddOrUpdate(TData entity)
        {
            lock (_lock)
            {
                AddOrUpdateInternal(entity);
            }

            await Collection.Save();

            return entity;
        }

        public virtual async Task BatchAddOrUpdate(TData[] entities)
        {
            lock (_lock)
            {
                foreach (var entity in entities)
                    AddOrUpdateInternal(entity);
            }

            await Collection.Save();
        }

        public virtual async Task Delete(Guid id)
        {
            var item = Collection.Data.FirstOrDefault(_ => _.Id == id);
            if (item == null)
            {
                throw new KeyNotFoundException("Can't find entity with id: " + id);
            }

            Collection.Data.Remove(item);
            await Collection.Save();
        }

        public virtual async Task BatchDelete(Predicate<TData> predicate)
        {
            Collection.Data.RemoveAll(predicate);
            await Collection.Save();
        }

        protected virtual void AddOrUpdateInternal(TData entity)
        {
            if (entity.Id.HasValue)
            {
                var item = Collection.Data.FirstOrDefault(_ => _.Id == entity.Id);
                var index = Collection.Data.IndexOf(item);
                if (index != -1)
                {
                    Collection.Data.RemoveAt(index);
                    Collection.Data.Insert(index, entity);
                }
            }
            else
            {
                entity.Id = Guid.NewGuid();
            }

            Collection.Data.Add(entity);
        }
    }
}
