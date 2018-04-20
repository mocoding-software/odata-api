using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.OData;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    public static class TypeExtensions
    {
        private const string Postfix = "Controller";

        public static Type GetODataResourceType(this TypeInfo controllerType, IODataApiBuilder modelBuilder)
        {
            var isODataController = typeof(ODataController).IsAssignableFrom(controllerType);
            if (!isODataController)
                return null;
            var crudController = FindCrudController(controllerType);

            // copied original code https://github.com/aspnet/Mvc/blob/c7559e1ff4fa6c6bffc855794f36089eed7fc4c2/src/Microsoft.AspNetCore.Mvc.Core/Internal/DefaultApplicationModelProvider.cs
            var controllerName = controllerType.Name.EndsWith(Postfix, StringComparison.OrdinalIgnoreCase) ?
                controllerType.Name.Substring(0, controllerType.Name.Length - Postfix.Length) :
                controllerType.Name;

            return crudController != null
                   ? crudController.GenericTypeArguments[0]
                   : modelBuilder.Types.FirstOrDefault(_ => _.Name == controllerName);
        }

        private static Type FindCrudController(Type type)
        {
            var current = type;
            var target = typeof(CrudController<>);
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == target)
                {
                    return current;
                }

                current = current.BaseType;
            }

            return null;
        }
    }
}
