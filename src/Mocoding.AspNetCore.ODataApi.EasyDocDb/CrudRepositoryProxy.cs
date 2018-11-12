using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    internal class CrudRepositoryProxy<TEntity, TKey> : ICrudRepository<TEntity, TKey>
        where TEntity : class, new()
    {
        private ICrudRepository<TEntity, TKey> _repository;

        public CrudRepositoryProxy(DocumentRepositoryFactory factory, IEntityKeyAccossor keyAccossor)
        {
            _repository = factory.Get<TEntity, TKey>(keyAccossor);
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
