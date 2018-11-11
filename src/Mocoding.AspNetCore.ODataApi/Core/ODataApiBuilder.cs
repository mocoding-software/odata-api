using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
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

        public IEdmModel GetEdmModel()
        {
            return ODataModelBuilder.GetEdmModel();
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

            var entityType = ODataModelBuilder.EntitySet<T>(route).EntityType;
            entityType.HasKey(_ => _.Id);
            entityType.Property(_ => _.Id).IsOptional();

            _services.AddSingleton(repository ?? _factory.Create<T>(type.Name.ToLower()));

            return this;
        }

        public IODataApiBuilder AddResource<T>(string customRoute, string customSourceName)
            where T : class, IEntity, new()
        {
            if (string.IsNullOrEmpty(customRoute))
                throw new ArgumentException(nameof(customRoute));
            if (string.IsNullOrEmpty(customSourceName))
                throw new ArgumentException(nameof(customSourceName));

            return AddResource(customRoute, _factory.Create<T>(customSourceName));
        }

        public IODataApiBuilder AddResource(Type type)
        {
            var route = type.Name.ToLower();

            _types.Add(type);
            _routes.Add(type, route);

            var entityType = ODataModelBuilder.AddEntityType(type);
            ODataModelBuilder.AddEntitySet(type.Name, entityType);

            return this;
        }

        public string MapRoute(Type entityType)
        {
            return _routes[entityType];
        }
    }
}