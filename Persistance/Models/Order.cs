using static Persistance.DTOs.Orders.Enums;

namespace Persistance.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<OrderedProduct> OrderedProducts { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
