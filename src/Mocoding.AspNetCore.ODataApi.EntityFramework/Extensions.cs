using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Mocoding.AspNetCore.ODataApi.EntityFramework
{
    public static class Extensions
    {
        public static IODataApiBuilder AddResources<TContext>(this IODataApiBuilder builder) where TContext : DbContext
        {
            var properties = typeof(TContext).GetProperties();
            var dbSetType = typeof(DbSet<>);
            foreach (var property in properties.Where(_=> _.PropertyType.IsGenericType && _.PropertyType.GetGenericTypeDefinition() == dbSetType))
            {
                var type = property.PropertyType.GenericTypeArguments[0];
                builder.AddResource(type, property.Name.ToLower());
            }


            return builder;
        }

        public static IServiceCollection AddEntityFrameworkGenericRepository<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> configure)
            where TContext : DbContext
        {
            services
                .AddDbContext<TContext>(configure)
                .TryAddScoped(typeof(ICrudRepository<,>), typeof(DbSetRepository<,>));
            return services;
        }


    }
}
