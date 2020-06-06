using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mocoding.AspNetCore.ODataApi;
using Mocoding.AspNetCore.ODataApi.EntityFramework;
using WebApp.EF.Models;

namespace WebApp.EF
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkGenericRepository<EmDbContext>(options =>
                options.UseSqlServer("Server=.;Database=EmDb;User=sa;Password=<pass>"));

            services
                .AddMvcCore()
                .AddApiExplorer()
                .AddODataApi()
                    .AddResources<EmDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseODataApi();
        }
    }
}
