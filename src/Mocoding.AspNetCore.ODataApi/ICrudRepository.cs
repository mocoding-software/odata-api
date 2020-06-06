using System.Linq;
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
    }
}
