using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Mvc;

namespace Mocoding.AspNetCore.ODataApi
{
    public class CrudController<TEntity, TKey> : ODataController
        where TEntity : class, new()
    {
        public CrudController(ICrudRepository<TEntity, TKey> repository)
        {
            Repository = repository;
        }

        protected ICrudRepository<TEntity, TKey> Repository { get; }

        [EnableQuery]
        public IQueryable<TEntity> Get()
        {
            return Repository.QueryRecords();
        }

        public virtual async Task<IActionResult> Get([FromODataUri] TKey key)
        {
            var entity = await Repository.FindByKey(key);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        public virtual async Task<CreatedODataResult<TEntity>> Post([FromBody]TEntity entity)
        {
            await Repository.AddOrUpdate(entity);
            return base.Created(entity);
        }

        public virtual async Task<UpdatedODataResult<TEntity>> Put(TKey key, [FromBody]TEntity entity)
        {
            await Repository.AddOrUpdate(entity);
            return base.Updated(entity);
        }

        public virtual async Task<UpdatedODataResult<TEntity>> Patch(TKey key, [FromBody]Delta<TEntity> patch)
        {
            var entity = await Repository.FindByKey(key);
            if (entity == null)
                throw new KeyNotFoundException();

            patch.CopyChangedValues(entity);
            return base.Updated(entity);
        }

        public virtual async Task Delete(TKey key)
        {
            await Repository.DeleteByKey(key);
        }
    }
}
