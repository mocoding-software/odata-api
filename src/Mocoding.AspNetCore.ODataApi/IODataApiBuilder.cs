using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi
{
    public interface IODataApiBuilder
    {
        IODataApiBuilder UseFactory(IRepositoryFactory factory);
        IODataApiBuilder AddResource<T>(string customRoute = null)
            where T : class, IEntity, new();
    }
}