using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Client;
using Mocoding.AspNetCore.ODataApi.EasyDocDb;
using Mocoding.AspNetCore.ODataApi.Tests.Factories;
using Xunit;

namespace Mocoding.AspNetCore.ODataApi.Tests.Integration
{
    public class EasyDocDbExtensionsTests : IClassFixture<EasyDocDbWebAppFactory>
    {
        private readonly EasyDocDbWebAppFactory _factory;

        public EasyDocDbExtensionsTests(EasyDocDbWebAppFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public void EasyDocDbServicesTest()
        {
            Assert.NotNull(_factory.Services.GetRequiredService<DocumentRepositoryFactory>());
        }
    }
}
