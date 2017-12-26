using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mocoding.AspNetCore.OdataApi.Core
{
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
            foreach (var entityType in _modelBuilder.Types)
            {
                var typeName = entityType.Name + "Controller";
                if (feature.Controllers.Any(t => t.Name == typeName))
                    continue;

                // There's no 'real' controller for this entity, so add the generic version.
                var controllerType = typeof(CrudController<>)
                    .MakeGenericType(entityType).GetTypeInfo();
                feature.Controllers.Add(controllerType);
            }
        }
    }
}
