using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Mocoding.AspNetCore.ODataApi.Core
{
    internal interface IModelMetadataProvider
    {
        IEdmModel GetEdmModel();

        IEnumerable<EntityMetadata> GetModelMetadata();
    }
}
