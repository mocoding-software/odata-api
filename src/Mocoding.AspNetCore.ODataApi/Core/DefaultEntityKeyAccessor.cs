using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mocoding.AspNetCore.ODataApi.Core;

namespace Mocoding.AspNetCore.ODataApi.DataAccess
{
    internal class DefaultEntityKeyAccessor : IEntityKeyAccessor
    {
        private readonly IDictionary<Type, PropertyInfo> _mapping;

        public DefaultEntityKeyAccessor(IModelMetadataProvider metadataProvider)
        {
            _mapping = metadataProvider.GetEdmModel().GetEntityKeyMapping().ToDictionary(_ => _.Key, _ => _.Value);
        }

        public TKey GetKey<TEntity, TKey>(TEntity entity)
        {
            var entityType = typeof(TEntity);
            var keyProperty = _mapping[entityType];
            return (TKey)keyProperty.GetValue(entity);
        }
    }
}
