using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    /// <summary>
    /// This class is used to populate generic controllers for all resource types
    /// except those that have already a controller defined.
    /// </summary>
    internal class CrudControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly ODataApiBuilder _modelBuilder;

        public CrudControllerFeatureProvider(ODataApiBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            // This is designed to run after the default ControllerTypeProvider,
            // so the list of 'real' controllers has already been populated.
            var existingResourceTypes = feature.Controllers
                    .Select(_ => _.GetODataResourceType(_modelBuilder))
                    .Where(_ => _ != null).ToArray();
            var newResourceTypes = _modelBuilder.Types.Except(existingResourceTypes);

            // There's no 'real' controller for this entity, so add the generic version.
            foreach (var entityType in newResourceTypes)
            {
                if (!entityType.IsSubclassOf(typeof(IEntity)))
                    continue;
                var controllerType = typeof(CrudController<>)
                    .MakeGenericType(entityType).GetTypeInfo();
                feature.Controllers.Add(controllerType);
            }
        }
    }
}
