using System;
using Mocoding.AspNetCore.OdataApi.DataAccess;
using Mocoding.AspNetCore.OdataApi.EasyDocDB.Helpers;

namespace WebApp
{
    [ReadOptimized]
    public class Users : IEntity
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }
    }
}