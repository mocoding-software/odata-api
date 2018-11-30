using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.OData.Edm;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    internal class EntityMetadata
    {
        public EntityMetadata(Type entityType, string route)
        {
            EntityType = entityType;
            Route = route;
        }

        public Type EntityType { get; }
        public PropertyInfo EntityKey { get; private set; }
        public string Route { get; }

        public void SetKey(IEdmEntityType edmType)
        {
            var key = edmType.DeclaredKey.First();
            EntityKey = EntityType.GetProperty(key.Name) ?? EntityType.GetProperty(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(key.Name));
        }
    }
}
