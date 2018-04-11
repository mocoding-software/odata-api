using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;

namespace WebApp
{
    /// <summary>
    /// Example of custom controller that overrides ODataController.
    /// </summary>
    public class OrderController : ODataController
    {
        [EnableQuery]
        public IQueryable<Order> Get()
        {
            var list = new List<Order>()
            {
                new Order() { Id = Guid.Empty, Title = "hello from custom controller" }
            };

            return list.AsQueryable();
        }
    }
}
