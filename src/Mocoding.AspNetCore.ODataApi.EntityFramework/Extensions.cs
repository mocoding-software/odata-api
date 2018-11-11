using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace Mocoding.AspNetCore.ODataApi.EntityFramework
{
    public static class Extensions
    {
        public static IODataApiBuilder AddEntityFramework<TContext>(this IODataApiBuilder builder) where TContext : IDbContextDependencies, new()
        {
            var context = new TContext();
            var types = context.Model.GetEntityTypes();
            foreach (var entityType in types)
            {
                var type = entityType.ClrType;
                var name = entityType.Name.Split('.').Last();
                var route = name.ToLower();
                builder.AddResource(type);
            }
            return builder; //builder.UseFactory(new MongoDbFactory(connection));
        }
    }
}
