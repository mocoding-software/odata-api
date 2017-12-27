using System.Linq;
using Microsoft.AspNet.OData.Builder;
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

        public static IRouteBuilder UseOdata(this IRouteBuilder routeBuilder, IApplicationBuilder app)
        {
            var builder = new ODataConventionModelBuilder(app.ApplicationServices);
            var usedTypes = app.ApplicationServices.GetService<ODataApiBuilder>().GetUsedTypes();
            foreach (var usedType in usedTypes)
            {
                builder.AddEntityType(usedType);
            }

            routeBuilder.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            routeBuilder.MapODataServiceRoute($"", null, builder.GetEdmModel());

            return routeBuilder;
        }
    }
}