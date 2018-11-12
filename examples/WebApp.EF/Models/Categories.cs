using System;
using System.Collections.Generic;

namespace WebApp.EF.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Transactions = new HashSet<Transactions>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public ICollection<Transactions> Transactions { get; set; }
    }
}
