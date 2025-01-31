namespace Persistance.Models
{
    internal class OrderedProduct
    {
        public string OrderedProductId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
