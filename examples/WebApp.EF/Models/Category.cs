using System;
using System.Collections.Generic;

namespace WebApp.EF.Models
{
    public partial class Category
    {
        public Category()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
