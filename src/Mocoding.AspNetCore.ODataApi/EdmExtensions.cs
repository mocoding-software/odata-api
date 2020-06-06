using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNet.OData;
using Microsoft.OData.Edm;

namespace Mocoding.AspNetCore.ODataApi
{
    public static class EdmExtensions
    {
        public static IEnumerable<KeyValuePair<Type, PropertyInfo>> GetEntityKeyMapping(this IEdmModel model)
        {
            var entities = model.SchemaElements.Where(_ => _ is IEdmEntityType).Cast<IEdmEntityType>();

            // There's no 'real' controller for this entity, so add the generic version.
            foreach (var entity in entities)
            {
                var key = entity.DeclaredKey.First();
                var annotation = model.GetAnnotationValue<ClrTypeAnnotation>(entity);
                var entityType = annotation.ClrType;
                var keyProperty = entityType?.GetProperty(key.Name) ?? entityType?.GetProperty(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(key.Name));

                yield return new KeyValuePair<Type, PropertyInfo>(entityType, keyProperty);
            }
        }
    }
}
