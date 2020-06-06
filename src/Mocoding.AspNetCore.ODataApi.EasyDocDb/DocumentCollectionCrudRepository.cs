using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public class DocumentCollectionCrudRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey>
        where TEntity : class, new()
    {
        private readonly IEntityKeyAccessor _keyAccessor;

        public DocumentCollectionCrudRepository(IDocumentCollection<TEntity> collection, IEntityKeyAccessor keyAccessor)
        {
            _keyAccessor = keyAccessor;
            Collection = collection;
        }

        protected IDocumentCollection<TEntity> Collection { get; }

        public virtual IQueryable<TEntity> QueryRecords() => Collection.Documents.Select(_ => _.Data).AsQueryable();

        public virtual async Task<TEntity> AddOrUpdate(TEntity entity)
        {
            var id = _keyAccessor.GetKey<TEntity, TKey>(entity);
            if (id == null)
                throw new InvalidOperationException($"object key is missing");

            var item = GetById(id);
            if (item != null)
                await item.SyncUpdate(entity);
            else
                await Collection.New(entity).Save();

            return entity;
        }

        public Task<TEntity> FindByKey(TKey key)
        {
            return Task.FromResult(GetById(key)?.Data);
        }

        public async Task DeleteByKey(TKey key)
        {
            var item = GetById(key);
            if (item == null)
                throw new KeyNotFoundException("Can't find entity with key: " + key);
            await item.Delete();
        }

        // public virtual async Task BatchAddOrUpdate(TEntity[] entities)
        // {
        //     foreach (var entity in entities)
        //         await AddOrUpdate(entity);
        // }
        // public virtual async Task BatchDelete(Expression<Func<TEntity, bool>> predicate)
        // {
        //     var documents = Collection.Documents.Where(predicate.Compile() as Func<IDocument<TEntity>, bool>);
        //     foreach (var document in documents)
        //     {
        //         await document.Delete();
        //     }
        // }
        private IDocument<TEntity> GetById(TKey id) => Collection.Documents.FirstOrDefault(_ =>
            EqualityComparer<TKey>.Default.Equals(_keyAccessor.GetKey<TEntity, TKey>(_.Data), id));
    }
}
