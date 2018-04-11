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
        private readonly ODataApiBuilder _modelBuilder;

        public CrudControllerNameConvention(ODataApiBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Apply(ControllerModel controller)
        {
            var resourceType = controller.ControllerType.GetODataResourceType(_modelBuilder);
            if (resourceType == null)
                return;

            var route = _modelBuilder.MapRoute(resourceType);
            controller.ControllerName = route;
        }
    }
}
