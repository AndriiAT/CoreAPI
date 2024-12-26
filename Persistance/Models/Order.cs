using System;
using System.Collections.Generic;

namespace Persistance.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public string User { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public DateTime Date { get; set; }
    }
}
