using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Mocoding.AspNetCore.ODataApi.EntityFramework
{
    public static class Extensions
    {
        public static IODataApiBuilder AddEntityFramework<TContext>(this IODataApiBuilder builder) where TContext : DbContext, new()
        {
            var context = new TContext();
            var types = context.Model.GetEntityTypes();
            foreach (var entityType in types)
                builder.AddResource(entityType.ClrType);
            
            builder.Services.TryAddScoped(typeof(ICrudRepository<,>), typeof(DbSetRepository<,>));
            return builder; 
            
        }
    }
}
