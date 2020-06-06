using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApp;

namespace Mocoding.AspNetCore.ODataApi.Tests.Factories
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddODataApi()
                    .AddResource<User>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseODataApi();
        }
    }
}
