using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    // Used to set the controller name for routing purposes. Without this convention the
    // names would be like 'CrudController`1[Resource]' instead of 'Resource'.
    //
    // Conventions can be applied as attributes or added to MvcOptions.Conventions.
    internal class CrudControllerNameConvention : IControllerModelConvention
    {
        private readonly ODataApiBuilder _modelBuilder;

        public CrudControllerNameConvention(ODataApiBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsGenericType ||
                controller.ControllerType.GetGenericTypeDefinition() != typeof(CrudController<>))
            {
                // Not a CrudController, ignore.
                return;
            }

            var entityType = controller.ControllerType.GenericTypeArguments[0];
            var route = _modelBuilder.MapRoute(entityType);
            controller.ControllerName = route;
        }
    }
}
