using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mocoding.AspNetCore.ODataApi;
using Mocoding.AspNetCore.ODataApi.EasyDocDb;
using Mocoding.EasyDocDb;
using Mocoding.EasyDocDb.FileSystem;
using Mocoding.EasyDocDb.Json;

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
            services
                .AddSingleton<IDocumentStorage, EmbeddedStorage>()
                .AddSingleton<IDocumentSerializer, JsonSerializer>()
                .AddEasyDocDbGenericRepository(options => options.Connection = "../data")
                .AddMvcCore()
                .AddODataApi()
                    .AddResource<User>()
                    .AddResource<Role>("Roles") // custom Entity Name / Url
                    .AddResource<KeyValuePair>("settings") // override controller test 1
                    .AddResource<Order>(); // override controller test 2
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseODataApi();
        }
    }
}