using System;

namespace Mocoding.AspNetCore.OdataApi.DataAccess
{
    public interface IEntity
    {
        Guid? Id { get; set; }
    }
}
