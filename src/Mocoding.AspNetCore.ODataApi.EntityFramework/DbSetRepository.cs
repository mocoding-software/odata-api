using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;

namespace Mocoding.AspNetCore.ODataApi.EntityFramework
{
    class DbSetRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey>
        where TEntity : class, new()
    {
        private readonly DbSet<TEntity> _repository;
        private readonly DbContext _context;
        private readonly IEntityKeyAccossor _keyAccossor;

        public DbSetRepository(DbContext context, IEntityKeyAccossor keyAccossor)
        {
            _repository = context.Set<TEntity>();
            _context = context;
            _keyAccossor = keyAccossor;
        }
      
        public IQueryable<TEntity> QueryRecords()
        {
            return _repository;
        }

        public async Task<TEntity> AddOrUpdate(TEntity entity)
        {
            // var contains = ;
            //var id = _keyAccossor.GetKey<TEntity, TKey>(entity);
            //if (id == null)
            //    throw new InvalidOperationException($"object key is missing");

            //var dbEntity = await FindByKey(id);

            var result = _repository.Local.Contains(entity)
                ? _repository.Update(entity)
                : _repository.Add(entity);
            
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<TEntity> FindByKey(TKey key)
        {
            return await _repository.FindAsync(key);
        }

        public async Task DeleteByKey(TKey key)
        {
            var entity = await _repository.FindAsync(key);
            _repository.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
