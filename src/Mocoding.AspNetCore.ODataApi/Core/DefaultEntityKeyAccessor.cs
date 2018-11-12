using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    internal class DefaultEntityKeyAccessor : IEntityKeyAccossor
    {
        private readonly IEnumerable<EntityMetadata> _metadata;

        public DefaultEntityKeyAccessor(IModelMetadataProvider metadataProvider)
        {
            _metadata = metadataProvider.GetModelMetadata();
        }

        public TKey GetKey<TEntity, TKey>(TEntity entity)
        {
            var entityType = typeof(TEntity);
            var entityMetadata = _metadata.First(_ => _.EntityType == entityType);
            return (TKey)entityMetadata.EntityKey.GetValue(entity);
        }
    }
}
