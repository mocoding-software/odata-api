using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Mocoding.AspNetCore.ODataApi.Core;

namespace Mocoding.AspNetCore.ODataApi
{
    public interface IODataApiBuilder
    {
        IServiceCollection Services { get; }

        IODataApiBuilder AddResource<T>(string customRoute = null)
            where T : class;
        IODataApiBuilder AddResource(Type type, string customRoute = null);
    }
}