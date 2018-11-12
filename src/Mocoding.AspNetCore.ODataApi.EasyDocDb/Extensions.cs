using Microsoft.Extensions.DependencyInjection.Extensions;
using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.ODataApi.EasyDocDb
{
    public static class Extensions
    {
        public static IODataApiBuilder AddEasyDocDb(this IODataApiBuilder oDataApiBuilder, string folder = "../data")
        {
            oDataApiBuilder.Services.TryAddSingleton(new DocumentRepositoryFactory(folder));
            return oDataApiBuilder;
        }

        public static IODataApiBuilder AddEasyDocDb(this IODataApiBuilder oDataApiBuilder, string connection, IDocumentStorage storage)
        {
            oDataApiBuilder.Services.TryAddSingleton(new DocumentRepositoryFactory(connection, storage));
            return oDataApiBuilder;
        }
    }
}