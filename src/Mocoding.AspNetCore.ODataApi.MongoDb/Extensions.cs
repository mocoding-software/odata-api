using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddODataApiMongoDb(this IODataApiBuilder oDataApiBuilder, string connection)
        {
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            oDataApiBuilder.Services.TryAddSingleton<IMongoDatabase>(services =>
            {
                var urlBuilder = new MongoUrlBuilder(connection);
                var client = new MongoClient(connection);
                return client.GetDatabase(urlBuilder.DatabaseName);
            }); 

            return oDataApiBuilder;
        }
    }
}
