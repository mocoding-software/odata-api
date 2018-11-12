using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    /// <summary>
    /// Used to set the controller name for routing purposes. Without this convention the
    /// names would be like 'CrudController`1[Resource]' instead of 'Resource'.
    /// Conventions can be applied as attributes or added to MvcOptions.Conventions.
    /// </summary>
    internal class CrudControllerNameConvention : IControllerModelConvention
    {
        private readonly IModelMetadataProvider _metadataProvider;

        public CrudControllerNameConvention(IModelMetadataProvider metadataProvider)
        {
            _metadataProvider = metadataProvider;
        }

        public void Apply(ControllerModel controller)
        {
            var target = typeof(CrudController<,>);
            var controllerType = controller.ControllerType;
            var isCrudController = controllerType.IsGenericType && controllerType.GetGenericTypeDefinition() == target;
            if (!isCrudController)
                return;

            var entityType = controllerType.GenericTypeArguments[0];
            var metadata = _metadataProvider.GetModelMetadata();
            var entityMetadata = metadata.FirstOrDefault(_ => _.EntityType == entityType);
            if (entityMetadata != null)
                controller.ControllerName = entityMetadata.Route;
        }
    }
}
