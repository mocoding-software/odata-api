using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using Mocoding.AspNetCore.ODataApi.Core;

namespace Mocoding.AspNetCore.ODataApi
{
    public static class ODataApiExtensions
    {
        public static IODataApiBuilder AddODataApi(this IMvcCoreBuilder mvc)
        {
            var services = mvc.Services;
            var modelBuilder = new ODataApiBuilder(services);
            services.AddOData();
            services.AddSingleton<IModelMetadataProvider>(modelBuilder);
            services.TryAddSingleton<IEntityKeyAccossor, DefaultEntityKeyAccessor>();
            mvc.AddMvcOptions(options => options.Conventions.Add(new CrudControllerNameConvention(modelBuilder)))
               .ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new CrudControllerFeatureProvider(modelBuilder)));

            return modelBuilder;
        }

        public static IRouteBuilder UseOData(this IRouteBuilder routeBuilder, IApplicationBuilder app, string routePrfix = ODataApiOptions.DefaultRoute)
        {
            return routeBuilder.UseOData(app, new ODataApiOptions() { RoutePrfix = routePrfix });
        }

        public static IRouteBuilder UseOData(this IRouteBuilder routeBuilder, IApplicationBuilder app, ODataApiOptions options)
        {
            var apiBuilder = app.ApplicationServices.GetRequiredService<IModelMetadataProvider>();

            routeBuilder.Filter().Select().Expand().Count().OrderBy().MaxTop(null);
            routeBuilder.MapODataServiceRoute("OData", options.RoutePrfix, apiBuilder.GetEdmModel());

            return routeBuilder;
        }
    }
}