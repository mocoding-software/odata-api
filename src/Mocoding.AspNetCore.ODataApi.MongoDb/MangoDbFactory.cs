using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace Mocoding.AspNetCore.ODataApi.MongoDb
{
    class MangoDbFactory : IRepositoryFactory
    {
        private readonly string _conn;
        public MangoDbFactory(string connection)
        {
            _conn = connection;
        }

        public ICrudRepository<T> Create<T>(string name) where T : class, IEntity, new()
        {
            return new CrudRepository<T>(_conn, name);
        }
    }
}
