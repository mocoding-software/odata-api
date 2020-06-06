using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoDbGenericRepository<TContext>(this IServiceCollection services, string connection)
        {
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            services.TryAddSingleton<IMongoDatabase>(_ =>
                {
                    var urlBuilder = new MongoUrlBuilder(connection);
                    var client = new MongoClient(connection);
                    return client.GetDatabase(urlBuilder.DatabaseName);
                });
            services.TryAddScoped(typeof(ICrudRepository<,>), typeof(CrudRepository<,>));
            return services;
        }
    }
}
