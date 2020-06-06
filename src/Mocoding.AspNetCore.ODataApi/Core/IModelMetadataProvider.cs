using System.Collections.Generic;
using Microsoft.OData.Edm;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    internal interface IModelMetadataProvider
    {
        IEdmModel GetEdmModel();

        // IEnumerable<EntityMetadata> GetModelMetadata();
    }
}
