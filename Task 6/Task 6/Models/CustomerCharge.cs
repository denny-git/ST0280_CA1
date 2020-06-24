using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_6.Models
{
    public class CustomerCharge
    {
        public string Id { get; set; }
        public long Amount { get; set; }
        public long AmountRefunded { get; set; }
        public string Description { get; set; }
        public string ReceiptUrl { get; set; }
        public string Status { get; set; }
        public bool Refunded { get; set; }
        public DateTime? Created { get; set; }
    }
}
