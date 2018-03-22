namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddODataApiEasyDocDb(this IODataApiBuilder oDataApiBuilder, string foder = "../data")
        {
            return oDataApiBuilder.UseFactory(new Factory(foder));
        }
    }
}