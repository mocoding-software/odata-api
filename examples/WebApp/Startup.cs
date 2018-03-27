using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mocoding.AspNetCore.ODataApi;
using Mocoding.AspNetCore.ODataApi.EasyDocDb;
using Newtonsoft.Json.Serialization;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services
                .AddMvcCore()
                .AddJsonFormatters(settings => settings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddApiExplorer()
                .AddODataApi()
                    .AddODataApiEasyDocDb()
                    .AddResource<User>()
                    .AddResource<Role>("Roles"); // custom Entity Name / Url

            services.AddSwaggerSpecification();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            app.Map("/api", apiApp =>
            {
                apiApp.UseSwaggerUIAndSpec();
                apiApp.UseMvc(builder => builder.UseOData(app));
            });

            app.UseStaticFiles();
        }
    }
}

