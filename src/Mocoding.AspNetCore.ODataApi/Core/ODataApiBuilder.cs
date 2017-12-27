using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    public class ODataApiBuilder : IODataApiBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IList<Type> _types;
        private readonly IDictionary<Type, string> _routes;
        private IRepositoryFactory _factory;

        public ODataApiBuilder(IServiceCollection services)
        {
            _services = services;
            _types = new List<Type>();
            _routes = new Dictionary<Type, string>();
        }

        public IEnumerable<Type> Types => _types;

        public IODataApiBuilder UseFactory(IRepositoryFactory factory)
        {
            _factory = factory;
            return this;
        }

        public IEnumerable<Type> GetUsedTypes()
        {
            return _types.ToList().AsReadOnly();
        }

        public IODataApiBuilder AddResource<T>(string customRoute = null)
            where T : class, IEntity, new()
        {
            var type = typeof(T);
            var name = type.Name.ToLower();
            _types.Add(type);
            _routes.Add(type, customRoute ?? name);

            _services.AddSingleton(_factory.Create<T>(name));

            return this;
        }

        public string MapRoute(Type entityType)
        {
            return _routes[entityType];
        }
    }
}