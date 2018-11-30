using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    /// <summary>
    /// This class is used to populate generic controllers for all resource types
    /// except those that have already a controller defined.
    /// </summary>
    internal class CrudControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IModelMetadataProvider _metadataProvider;

        public CrudControllerFeatureProvider(IModelMetadataProvider metadataProvider)
        {
            _metadataProvider = metadataProvider;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var model = _metadataProvider.GetModelMetadata();

            // There's no 'real' controller for this entity, so add the generic version.
            foreach (var entityType in model)
            {
                var controllerType = typeof(CrudController<,>)
                    .MakeGenericType(entityType.EntityType, entityType.EntityKey.PropertyType).GetTypeInfo();
                feature.Controllers.Add(controllerType);
            }
        }
    }
}
