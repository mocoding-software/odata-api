using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    internal class CrudRepositoryProxy<TEntity, TKey> : ICrudRepository<TEntity, TKey>
        where TEntity : class, new()
    {
        private readonly ICrudRepository<TEntity, TKey> _repository;

        public CrudRepositoryProxy(DocumentRepositoryFactory factory)
        {
            _repository = factory.Create<TEntity, TKey>();
        }

        public Task<TEntity> AddOrUpdate(TEntity entity)
        {
            return _repository.AddOrUpdate(entity);
        }

        public Task DeleteByKey(TKey key)
        {
            return _repository.DeleteByKey(key);
        }

        public Task<TEntity> FindByKey(TKey key)
        {
            return _repository.FindByKey(key);
        }

        public IQueryable<TEntity> QueryRecords()
        {
            return _repository.QueryRecords();
        }
    }
}
