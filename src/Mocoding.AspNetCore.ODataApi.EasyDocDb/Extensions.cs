namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddOdataApiEasyDocDb(this IODataApiBuilder oDataApiBuilder, string foder = "")
        {
            return oDataApiBuilder.UseFactory(new Factory(foder == string.Empty ? "../data" : "../data" + "/" + foder));
        }
    }
}