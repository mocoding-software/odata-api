using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mocoding.AspNetCore.OdataApi.DataAccess;
using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.OdataApi.EasyDocDB
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

        public async Task ReInitRepository(List<TData> list)
        {
            Collection.Data.RemoveAll(_ => _.Id != null);
            Collection.Data.AddRange(list);
            await Collection.Save();
        }

        public IQueryable<TData> GetAll()
        {
            return Collection.Data.AsQueryable();
        }

        public async Task<TData> AddOrUpdate(TData entity)
        {
            lock (_lock)
            {
                AddOrUpdateInternal(entity);
            }

            await Collection.Save();

            return entity;
        }

        public async Task Delete(Guid id)
        {
            var item = Collection.Data.FirstOrDefault(_ => _.Id == id);
            if (item == null)
            {
                throw new NullReferenceException("Can't find entity with id: " + id);
            }

            Collection.Data.Remove(item);
            await Collection.Save();
        }

        public async Task DeleteAll()
        {
            Collection.Data.Clear();
            await Collection.Save();
        }

        private void AddOrUpdateInternal(TData entity)
        {
            if (entity.Id.HasValue)
            {
                var item = Collection.Data.FirstOrDefault(_ => _.Id == entity.Id);
                var index = Collection.Data.IndexOf(item);
                Collection.Data.RemoveAt(index);
                Collection.Data.Insert(index, entity);
            }
            else
            {
                entity.Id = Guid.NewGuid();
                Collection.Data.Add(entity);
            }
        }
    }
}
