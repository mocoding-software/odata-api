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

            // var types = model.Select(_ => _.EntityType).ToArray();
            // This is designed to run after the default ControllerTypeProvider,
            // so the list of 'real' controllers has already been populated.
            // var existingResourceTypes = feature.Controllers
            //        .Select(_ => _.GetODataResourceType(_metadataProvider, types))
            //        .Where(_ => _ != null).ToArray();
            // var newResourceTypes = types.Except(existingResourceTypes);

            //// There's no 'real' controller for this entity, so add the generic version.
            foreach (var entityType in model)
            {
                var controllerType = typeof(CrudController<,>)
                    .MakeGenericType(entityType.EntityType, entityType.EntityKey.PropertyType).GetTypeInfo();
                feature.Controllers.Add(controllerType);
            }
        }
    }
}
