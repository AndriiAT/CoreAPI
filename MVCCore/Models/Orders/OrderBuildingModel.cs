using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCCore.Models.Orders
{
    public class OrderBuildingModel
    {
        [JsonProperty("products")]
        public List<OrderProductBuildingModel> Products { get; set; }
    }

    public class OrderProductBuildingModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("count")]
        [Range(1, int.MaxValue, ErrorMessage = "Count must be greater than 0")]
        public int Count { get; set; }
    }
}
