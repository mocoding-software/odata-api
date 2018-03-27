using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton(modelBuilder);
            mvc.AddMvcOptions(options => options.Conventions.Add(new CrudControllerNameConvention(modelBuilder)))
               .ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new CrudControllerFeatureProvider(modelBuilder)));

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>()
                    .Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.test-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>()
                    .Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.test-odata"));
                }
            });

            return modelBuilder;
        }

        public static IRouteBuilder UseOData(this IRouteBuilder routeBuilder, IApplicationBuilder app, string routePrfix = "odata")
        {
            var apiBuilder = app.ApplicationServices.GetService<ODataApiBuilder>();

            routeBuilder.Filter().Select().Expand().Count().MaxTop(null);
            routeBuilder.MapODataServiceRoute("OData", routePrfix, apiBuilder.ODataModelBuilder.GetEdmModel());

            return routeBuilder;
        }
    }
}