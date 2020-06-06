using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebApp;
using Xunit;

namespace Mocoding.AspNetCore.ODataApi.Tests
{
    public class CrudControllerTests
    {
        [Fact]
        public void GetTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            controller.Get();

            repo.Received().QueryRecords();
        }

        [Fact]
        public async void GetByIdTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            var user = new User() { Name = "Test"};
            repo.FindByKey(Arg.Is(Guid.Empty)).Returns(user);

            var result = await controller.Get(Guid.Empty) as OkObjectResult;

            Assert.Same(user, result?.Value);
        }

        [Fact]
        public async void GetByIdNotFoundTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            User user = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            repo.FindByKey(Arg.Is(Guid.Empty)).Returns(user);

            var result = await controller.Get(Guid.Empty) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async void PostTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            var user = new User();

            var result = await controller.Post(user);

            Assert.NotNull(result);
            await repo.Received().AddOrUpdate(Arg.Is(user));
        }

        [Fact]
        public async void PutTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            var user = new User();
            repo.FindByKey(Arg.Is(Guid.Empty)).Returns(user);
            var result = await controller.Put(Guid.Empty, new Delta<User>(typeof(User)));

            Assert.NotNull(result);
            await repo.Received().AddOrUpdate(Arg.Is(user));
        }

        [Fact]
        public async void PutNotFoundTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            User user = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            repo.FindByKey(Arg.Is(Guid.Empty)).Returns(user);
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await controller.Put(Guid.Empty, new Delta<User>(typeof(User))));
        }

        [Fact]
        public async void PatchTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            var user = new User();
            repo.FindByKey(Arg.Is(Guid.Empty)).Returns(user);
            var result = await controller.Patch(Guid.Empty, new Delta<User>(typeof(User)));

            Assert.NotNull(result);
            await repo.Received().AddOrUpdate(Arg.Is(user));
        }

        [Fact]
        public async void PatchNotFoundTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);

            User user = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            repo.FindByKey(Arg.Is(Guid.Empty)).Returns(user);
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await controller.Patch(Guid.Empty, new Delta<User>(typeof(User))));
        }

        [Fact]
        public async void DeleteTest()
        {
            var repo = Substitute.For<ICrudRepository<User, Guid>>();
            var controller = new CrudController<User, Guid>(repo);


            await controller.Delete(Guid.Empty);

            await repo.Received().DeleteByKey(Guid.Empty);
        }
    }
}
