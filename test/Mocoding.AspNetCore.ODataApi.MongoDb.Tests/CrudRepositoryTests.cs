using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.AspNetCore.ODataApi.DataAccess;
using Xunit;

namespace Mocoding.AspNetCore.ODataApi.MongoDb.Tests
{
    public class Data : IEntity
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        [Indexed]
        public int IndexedValue { get; set; }
    }

    public class CrudRepositoryTests
    {
        public ICrudRepository<Data> GetRepository()
        {
            return new CrudRepository<Data>("mongodb://localhost/test", "data");
        }

        [Fact]
        public async void SingleTest()
        {
            var repository = GetRepository();

            var test = await repository.AddOrUpdate(new Data()
            {
                Name = "Test"
            });

            var retrieved = repository.QueryRecords().FirstOrDefault(_ => _.Id == test.Id);

            Assert.NotNull(retrieved);
            Assert.Equal(test.Name, retrieved.Name);

            await repository.Delete(retrieved.Id.Value);

            retrieved = repository.QueryRecords().FirstOrDefault(_ => _.Id == test.Id);
            Assert.Null(retrieved);
        }

        [Fact]
        public async void ManyTest()
        {
            var repository = GetRepository();
            
            await DeleteAll(repository);

            await repository.BatchAddOrUpdate(Enumerable.Range(1, 10000)
                .Select(_ => new Data() { IndexedValue = _, Name = $"Item {_}" })
                .ToArray());

            var count = repository.QueryRecords().Count(_ => _.IndexedValue > 0);
            
            Assert.Equal(10000, count);

            await repository.BatchDelete(_=>_.IndexedValue > 5000);

            count = repository.QueryRecords().Count(_ => _.IndexedValue > 5000);
            Assert.Equal(0, count);

            await DeleteAll(repository);
        }

        private static async Task DeleteAll(ICrudRepository<Data> repository)
        {
            await repository.BatchDelete(_ => true);
            var count = repository.QueryRecords().Count();
            Assert.Equal(0, count);
        }
    }
}
