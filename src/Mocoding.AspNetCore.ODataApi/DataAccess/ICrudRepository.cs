using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.AspNetCore.OdataApi.DataAccess
{
    public interface ICrudRepository<T>
        where T : class, IEntity, new()
    {
        IQueryable<T> GetAll();
        Task<T> AddOrUpdate(T entity);
        Task Delete(Guid id);
    }
}
