using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    internal class ODataApiBuilder : IODataApiBuilder, IModelMetadataProvider
    {
        private readonly ODataConventionModelBuilder _modelBuilder;
        private IEdmModel _model;

        public ODataApiBuilder(bool enableLowerCamelCase)
        {
            _modelBuilder = new ODataConventionModelBuilder();
            if (enableLowerCamelCase)
                _modelBuilder.EnableLowerCamelCase();
        }

        public IEdmModel GetEdmModel()
        {
            return _model ??= _modelBuilder.GetEdmModel();
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
            var entityType = _modelBuilder.AddEntityType(type);
            _modelBuilder.AddEntitySet(route, entityType);
            return this;
        }
    }
}