using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public static class Extensions
    {
        public static IServiceCollection AddEasyDocDbGenericRepository(this IServiceCollection services, Action<ODataEasyDocDbOptions> configure)
        {
            services
                .Configure(configure)
                .AddSingleton<DocumentRepositoryFactory>()
                .AddSingleton<IRepository, Repository>()
                .TryAddScoped(typeof(ICrudRepository<,>), typeof(CrudRepositoryProxy<,>));

            return services;
        }
    }
}