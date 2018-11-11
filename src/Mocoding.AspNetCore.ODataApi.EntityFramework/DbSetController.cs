using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mocoding.AspNetCore.ODataApi.EntityFramework
{
    public class DbSetController<TEntity, TKey> : ODataController
        where TEntity : class
    {
        private readonly DbSet<TEntity> _repository;
        private readonly DbContext _context;

        public DbSetController(DbContext context)
        {
            _repository = context.Set<TEntity>();
            _context = context;
        }

        [EnableQuery]
        public IQueryable<TEntity> Get()
        {
            return _repository;
        }

        public virtual IActionResult Get([FromODataUri] TKey key)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<CreatedODataResult<TEntity>> Post([FromBody]TEntity entity)
        {
            var created = await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();
            return base.Created(created.Entity);
        }

        public virtual async Task<UpdatedODataResult<TEntity>> Put(TKey key, [FromBody]TEntity entity)
        {
            _repository.Update(entity);
            await _context.SaveChangesAsync();
            return base.Updated(entity);
        }

        public virtual async Task<UpdatedODataResult<TEntity>> Patch(TKey key, [FromBody]Delta<TEntity> patch)
        {
            var entity = await _repository.FindAsync(key);
            if (entity == null)
                throw new KeyNotFoundException();

            patch.CopyChangedValues(entity);
            await _context.SaveChangesAsync();
            return base.Updated(entity);
        }

        public virtual async Task Delete(TKey key)
        {
            var entity = await _repository.FindAsync(key);
            if (entity == null)
                throw new KeyNotFoundException();
            _repository.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
