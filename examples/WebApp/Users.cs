using System;
using Mocoding.AspNetCore.ODataApi.DataAccess;
using Mocoding.AspNetCore.ODataApi.EasyDocDb.Helpers;

namespace WebApp
{
    [ReadOptimized]
    public class Users : IEntity
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }
    }
}