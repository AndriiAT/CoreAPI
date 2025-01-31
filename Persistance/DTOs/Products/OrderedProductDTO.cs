namespace Persistance.DTOs.Products
{
    public class OrderedProductDTO
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
