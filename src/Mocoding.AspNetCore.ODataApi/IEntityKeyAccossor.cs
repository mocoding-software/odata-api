using System;
using System.Collections.Generic;
using System.Text;

namespace Mocoding.AspNetCore.ODataApi
{
    public interface IEntityKeyAccossor
    {
        TKey GetKey<TEntity, TKey>(TEntity entity);
    }
}
