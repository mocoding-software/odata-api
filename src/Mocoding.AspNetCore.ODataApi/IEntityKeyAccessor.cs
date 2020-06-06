namespace Mocoding.AspNetCore.ODataApi
{
    public interface IEntityKeyAccessor
    {
        TKey GetKey<TEntity, TKey>(TEntity entity);
    }
}
