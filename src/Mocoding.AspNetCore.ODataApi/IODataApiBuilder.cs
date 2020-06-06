using System;
using Microsoft.Extensions.DependencyInjection;

namespace Mocoding.AspNetCore.ODataApi
{
    public interface IODataApiBuilder
    {
        IODataApiBuilder AddResource<T>(string customRoute = null)
            where T : class;
        IODataApiBuilder AddResource(Type type, string customRoute = null);
    }
}