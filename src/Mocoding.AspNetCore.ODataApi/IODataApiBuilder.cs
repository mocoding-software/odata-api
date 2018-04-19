using System;
using System.Collections.Generic;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi
{
    public interface IODataApiBuilder
    {
        IEnumerable<Type> Types { get; }
        IODataApiBuilder UseFactory(IRepositoryFactory factory);
        IODataApiBuilder AddResource<T>(string customRoute = null, ICrudRepository<T> customRepostiory = null)
            where T : class, IEntity, new();
        IODataApiBuilder AddResource<T>(string customRoute, string customSourceName)
            where T : class, IEntity, new();
    }
}