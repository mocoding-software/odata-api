using MongoDB.Driver;

namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddODataApiMongoDb(this IODataApiBuilder oDataApiBuilder, string connection)
        {
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            return oDataApiBuilder.UseFactory(new MongoDbFactory(connection));
        }
    }
}
