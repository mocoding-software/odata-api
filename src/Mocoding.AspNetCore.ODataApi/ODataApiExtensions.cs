using System;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mocoding.AspNetCore.ODataApi.Core;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi
{
    public static class ODataApiExtensions
    {
        /// <summary>
        /// Adds required services to support OData API.
        /// </summary>
        /// <param name="mvc">The MVC.</param>
        /// <param name="enableLowerCamelCase">if set to <c>true</c> [enable lower camel case].</param>
        /// <returns>Builder to manipulate resources.</returns>
        public static IODataApiBuilder AddODataApi(this IMvcCoreBuilder mvc, bool enableLowerCamelCase = true)
        {
            var services = mvc.Services;
            var modelBuilder = new ODataApiBuilder(enableLowerCamelCase);
            services.AddOData();
            services.AddSingleton<IModelMetadataProvider>(modelBuilder);
            services.TryAddSingleton<IEntityKeyAccessor, DefaultEntityKeyAccessor>();
            mvc.AddMvcOptions(options => options.Conventions.Add(new CrudControllerNameConvention(modelBuilder)))
               .ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new CrudControllerFeatureProvider(modelBuilder)));

            return modelBuilder;
        }

        /// <summary>
        /// Adds OData API middleware to the pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="routePrefix">The route prefix.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseODataApi(this IApplicationBuilder app, string routePrefix = ODataApiOptions.DefaultRoute)
        {
            return app.UseODataApi(new ODataApiOptions() { RoutePrefix = routePrefix }, null);
        }

        /// <summary>
        /// Adds OData API middleware to the pipeline with additional customization options
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">The options.</param>
        /// <param name="configure">An <see cref="Action{IEndpointRouteBuilder}"/> to configure the provided <see cref="IEndpointRouteBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseODataApi(this IApplicationBuilder app, ODataApiOptions options, Action<IEndpointRouteBuilder> configure)
        {
            var apiBuilder = app.ApplicationServices.GetRequiredService<IModelMetadataProvider>();

            return app
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.Filter().Select().Expand().Count().OrderBy().MaxTop(null);
                    endpoints.MapODataRoute("OData", options.RoutePrefix, apiBuilder.GetEdmModel());
                    endpoints.MapControllers();
                    configure?.Invoke(endpoints);
                });
        }
    }
}