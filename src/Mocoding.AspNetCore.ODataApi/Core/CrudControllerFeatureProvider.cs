using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OData.Edm;

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
            var model = _metadataProvider.GetEdmModel();
            var entities = model.GetEntityKeyMapping();

            // There's no 'real' controller for this entity, so add the generic version.
            foreach (var entity in entities)
            {
                var controllerType = typeof(CrudController<,>)
                    .MakeGenericType(entity.Key, entity.Value.PropertyType).GetTypeInfo();
                feature.Controllers.Add(controllerType);
            }
        }
    }
}
