using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Mocoding.AspNetCore.ODataApi.Tests.Factories;
using Xunit;

namespace Mocoding.AspNetCore.ODataApi.Tests.Integration
{
    public class ODataApiExtensionsTests : IClassFixture<Factory>
    {
        private readonly Factory _factory;

        public ODataApiExtensionsTests(Factory factory)
        {
            _factory = factory;
        }

        [Fact]
        public void ServicesTest()
        {
            Assert.NotNull(_factory.Services.GetRequiredService<IEntityKeyAccessor>());
        }
    }
}
