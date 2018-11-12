using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mocoding.AspNetCore.ODataApi
{
    public interface ICrudRepository<T, in TKey>
        where T : class
    {
        IQueryable<T> QueryRecords();
        Task<T> AddOrUpdate(T entity);
        Task<T> FindByKey(TKey key);
        Task DeleteByKey(TKey key);

        // Task BatchAddOrUpdate(T[] entities);
        // Task BatchDelete(Expression<Func<T, bool>> predicate);
    }
}
