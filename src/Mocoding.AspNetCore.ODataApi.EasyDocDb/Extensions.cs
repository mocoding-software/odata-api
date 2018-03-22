using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddODataApiEasyDocDb(this IODataApiBuilder oDataApiBuilder, string foder = "../data")
        {
            return oDataApiBuilder.UseFactory(new Factory(foder));
        }

        public static IODataApiBuilder AddOdataApiEasyDocDb(this IODataApiBuilder oDataApiBuilder, string connection, IDocumentStorage storage)
        {
            return oDataApiBuilder.UseFactory(new Factory(connection, storage));
        }
    }
}