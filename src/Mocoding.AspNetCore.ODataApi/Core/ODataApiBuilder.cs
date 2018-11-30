using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    internal class ODataApiBuilder : IODataApiBuilder, IModelMetadataProvider
    {
        private readonly ODataConventionModelBuilder _modelBuilder;
        private readonly IList<EntityMetadata> _metadata;
        private IEdmModel _model;

        public ODataApiBuilder(IServiceCollection services, bool enableLowerCamelCase)
        {
            Services = services;
            _modelBuilder = new ODataConventionModelBuilder();
            if (enableLowerCamelCase)
                _modelBuilder.EnableLowerCamelCase();
            _metadata = new List<EntityMetadata>();
        }

        public IServiceCollection Services { get; }

        public IEdmModel GetEdmModel()
        {
            return _model ?? (_model = GetEdmModelInternal());
        }

        public IEnumerable<EntityMetadata> GetModelMetadata()
        {
            return _metadata;
        }

        public IODataApiBuilder AddResource<T>(string customRoute = null)
            where T : class
        {
            var type = typeof(T);
            return AddResource(type, customRoute);
        }

        public IODataApiBuilder AddResource(Type type, string customRoute = null)
        {
            var route = customRoute ?? type.Name.ToLower();
            _metadata.Add(new EntityMetadata(type, route));
            var entityType = _modelBuilder.AddEntityType(type);
            _modelBuilder.AddEntitySet(route, entityType);
            return this;
        }

        private IEdmModel GetEdmModelInternal()
        {
            var model = _modelBuilder.GetEdmModel();
            var edmTypes = model.SchemaElements.Where(_ => _ is IEdmEntityType).Cast<IEdmEntityType>();
            foreach (var edmType in edmTypes)
            {
                var metadataType = _metadata.First(_ => _.EntityType.FullName == edmType.FullTypeName());
                metadataType?.SetKey(edmType);
            }

            return model;
        }
    }
}