using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Mocoding.AspNetCore.OdataApi.DataAccess;

namespace Mocoding.AspNetCore.OdataApi.Core
{
    [Route("[controller]")]
    internal class CrudController<TEntity>
        where TEntity : class, IEntity, new()
    {
        public CrudController(ICrudRepository<TEntity> repository)
        {
            Repository = repository;
        }

        [ActionContext]
        public ActionContext ActionContext { get; set; }

        protected ICrudRepository<TEntity> Repository { get; }

        [HttpGet]
        [EnableQuery]
        public IEnumerable<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

        [HttpPost]

        // [ModelStateValidationActionFilter]
        public virtual async Task<TEntity> Create([FromBody]TEntity entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            await Repository.AddOrUpdate(entity);
            return entity;
        }

        [HttpGet("{id}")]
        public virtual TEntity Read(Guid id)
        {
            var entity = Repository.GetAll().FirstOrDefault(_ => _.Id == id);
            if (entity == null)
                throw new ArgumentNullException();

            return entity;
        }

        [HttpPut("{id}")]

        // [ModelStateValidationActionFilter]
        public virtual async Task<TEntity> Update(Guid id, [FromBody]TEntity entity)
        {
            entity.Id = id;
            await Repository.AddOrUpdate(entity);
            return entity;
        }

        [HttpDelete("{id}")]
        public virtual async Task Delete(Guid id)
        {
            await Repository.Delete(id);
        }
    }
}
