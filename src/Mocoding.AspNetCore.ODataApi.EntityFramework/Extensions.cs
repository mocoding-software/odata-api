using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Mocoding.AspNetCore.ODataApi.EntityFramework
{
    public static class Extensions
    {
        public static IODataApiBuilder AddEntityFramework<TContext>(this IODataApiBuilder builder) where TContext : DbContext
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<TContext>();
            var types = context.Model.GetEntityTypes();
            foreach (var entityType in types)
                builder.AddResource(entityType.ClrType);
            
            builder.Services.TryAddTransient<DbContext, TContext>();
            builder.Services.TryAddScoped(typeof(ICrudRepository<,>), typeof(DbSetRepository<,>));
            return builder; 
            
        }
    }
}
