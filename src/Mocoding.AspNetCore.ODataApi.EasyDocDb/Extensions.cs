using System;
using Microsoft.Extensions.DependencyInjection;
using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public static class Extensions
    {
        public static IServiceCollection AddEasyDocDbGenericRepository(this IServiceCollection services, Action<ODataEasyDocDbOptions> configure)
        {
            return services
                .Configure(configure)
                .AddSingleton<DocumentRepositoryFactory>()
                .AddSingleton<IRepository, Repository>()
                .AddSingleton(typeof(ICrudRepository<,>), typeof(CrudRepositoryProxy<,>));
        }
    }
}