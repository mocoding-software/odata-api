using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.OData.Builder;

namespace Mocoding.AspNetCore.ODataApi
{
    public class ODataApiOptions
    {
        public const string DefaultRoute = "odata";

        public ODataApiOptions()
        {
            RoutePrfix = DefaultRoute;
        }

        public string RoutePrfix { get; set; }
    }
}
