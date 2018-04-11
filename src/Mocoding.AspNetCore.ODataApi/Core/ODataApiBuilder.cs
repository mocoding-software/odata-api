using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Builder;
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
            ODataModelBuilder = new ODataConventionModelBuilder();
        }

        public IEnumerable<Type> Types => _types;

        internal ODataConventionModelBuilder ODataModelBuilder { get; }

        public IODataApiBuilder UseFactory(IRepositoryFactory factory)
        {
            _factory = factory;
            return this;
        }

        public IODataApiBuilder AddResource<T>(string customRoute = null, ICrudRepository<T> repository = null)
            where T : class, IEntity, new()
        {
            var type = typeof(T);
            var route = customRoute ?? type.Name;
            _types.Add(type);
            _routes.Add(type, route);

            ODataModelBuilder.EntitySet<T>(route);
            _services.AddSingleton(repository ?? _factory.Create<T>(type.Name.ToLower()));

            return this;
        }

        public string MapRoute(Type entityType)
        {
            return _routes[entityType];
        }
    }
}