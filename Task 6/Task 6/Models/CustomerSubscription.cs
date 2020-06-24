using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_6.Models
{
    public class CustomerSubscription
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public long Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Interval { get; set; }
        public long IntervalCount { get; set; }
        public DateTime? ResumesAt { get; set; }
    }
}
