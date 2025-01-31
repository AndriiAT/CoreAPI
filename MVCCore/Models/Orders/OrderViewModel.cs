using Persistance.DTOs.Products;
using static Persistance.DTOs.Orders.Enums;
using System.Collections.Generic;
using System;
using MVCCore.Models.Products;

namespace MVCCore.Models.Orders
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public List<ProductViewModel> OrderedProducts { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
