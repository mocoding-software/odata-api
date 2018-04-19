namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddODataApiMangoDb(this IODataApiBuilder oDataApiBuilder, string connection)
        {
            return oDataApiBuilder.UseFactory(new MangoDbFactory(connection));
        }
    }
}
