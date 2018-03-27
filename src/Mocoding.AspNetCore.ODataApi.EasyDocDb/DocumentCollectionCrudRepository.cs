using System;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.AspNetCore.ODataApi.DataAccess;
using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public class DocumentCollectionCrudRepository<TData> : ICrudRepository<TData>
        where TData : class, IEntity, new()
    {
        public DocumentCollectionCrudRepository(IDocumentCollection<TData> collection)
        {
            Collection = collection;
        }

        protected IDocumentCollection<TData> Collection { get; }

        public IQueryable<TData> QueryRecords() => Collection.Documents.Select(_ => _.Data).AsQueryable();

        public async Task<TData> AddOrUpdate(TData entity)
        {
            if (entity.Id.HasValue)
            {
                var item = GetById(entity.Id.Value);
                if (item != null)
                {
                    await item.SyncUpdate(entity);
                    return entity;
                }
            }
            else
            {
                entity.Id = Guid.NewGuid();
            }

            await Collection.New(entity).Save();

            return entity;
        }

        public async Task BatchAddOrUpdate(TData[] entities)
        {
            foreach (var entity in entities)
                await AddOrUpdate(entity);
        }

        public async Task Delete(Guid id)
        {
            var item = GetById(id);
            if (item == null)
                throw new NullReferenceException("Can't find entity with id: " + id);
            await item.Delete();
        }

        public async Task BatchDelete(Predicate<TData> predicate)
        {
            var documents = Collection.Documents.Where(_ => predicate(_.Data));
            foreach (var document in documents)
            {
                await document.Delete();
            }
        }

        private IDocument<TData> GetById(Guid id) => Collection.Documents.FirstOrDefault(_ => _.Data.Id == id);
    }
}
