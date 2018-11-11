using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using Mocoding.AspNetCore.ODataApi.EntityFramework;
using WebApp.EF.Models;

namespace WebApp.EF
{
    public class CategoriesController
        : DbSetController<Categories, Guid>
    {
        public CategoriesController(DbContext context) : base(context)
        {
        }
    }
}
