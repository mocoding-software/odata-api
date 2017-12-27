using System;

namespace Mocoding.AspNetCore.ODataApi.DataAccess
{
    public interface IEntity
    {
        Guid? Id { get; set; }
    }
}
