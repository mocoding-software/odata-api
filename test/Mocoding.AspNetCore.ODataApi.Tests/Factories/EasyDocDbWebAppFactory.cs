using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Mocoding.AspNetCore.ODataApi.Tests.Factories
{
    public class EasyDocDbWebAppFactory : WebApplicationFactory<WebApp.Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<WebApp.Startup>();
        }
    }
}
