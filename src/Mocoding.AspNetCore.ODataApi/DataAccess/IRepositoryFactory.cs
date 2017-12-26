namespace Mocoding.AspNetCore.OdataApi.DataAccess
{
    public interface IRepositoryFactory
    {
        ICrudRepository<T> Create<T>(string name)
            where T : class, IEntity, new();
    }
}
