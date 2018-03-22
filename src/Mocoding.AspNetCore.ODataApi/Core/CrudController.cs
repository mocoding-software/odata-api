using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    internal class CrudController<TEntity> : ODataController
        where TEntity : class, IEntity, new()
    {
        public CrudController(ICrudRepository<TEntity> repository)
        {
            Repository = repository;
        }

        [ActionContext]
        public ActionContext ActionContext { get; set; }

        protected ICrudRepository<TEntity> Repository { get; }

        [EnableQuery]
        public IQueryable<TEntity> Get()
        {
            return Repository.GetAll();
        }

        public virtual TEntity Get([FromODataUri] Guid key)
        {
            var entity = Repository.GetAll().FirstOrDefault(_ => _.Id == key);
            if (entity == null)
                throw new ArgumentNullException();

            return entity;
        }

        public virtual async Task<TEntity> Post([FromBody]TEntity entity)
        {
            await Repository.AddOrUpdate(entity);
            return entity;
        }

        public virtual async Task<TEntity> Put(Guid key, [FromBody]TEntity entity)
        {
            entity.Id = key;
            await Repository.AddOrUpdate(entity);
            return entity;
        }

        public virtual async Task<TEntity> Patch(Guid key, [FromBody]Delta<TEntity> moviePatch)
        {
            var entity = Repository.GetAll().FirstOrDefault(_ => _.Id == key);
            if (entity == null)
                throw new ArgumentNullException();

            moviePatch.CopyChangedValues(entity);
            await Repository.AddOrUpdate(entity);
            return entity;
        }

        public virtual async Task Delete(Guid key)
        {
            await Repository.Delete(key);
        }
    }
}
