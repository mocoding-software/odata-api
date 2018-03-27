using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.AspNetCore.ODataApi.DataAccess
{
    public interface ICrudRepository<T>
        where T : class, IEntity, new()
    {
        IQueryable<T> QueryRecords();
        Task<T> AddOrUpdate(T entity);
        Task Delete(Guid id);

        Task BatchAddOrUpdate(T[] entities);
        Task BatchDelete(Predicate<T> predicate);
    }
}
