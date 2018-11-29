using System;
using System.Collections.Generic;

namespace WebApp.EF.Models
{
    public partial class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public string Memo { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime PostedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? ImportJobId { get; set; }

        public Category Category { get; set; }
    }
}
