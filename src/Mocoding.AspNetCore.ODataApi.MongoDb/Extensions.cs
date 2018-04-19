namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddODataApiMongoDb(this IODataApiBuilder oDataApiBuilder, string connection)
        {
            return oDataApiBuilder.UseFactory(new MongoDbFactory(connection));
        }
    }
}
