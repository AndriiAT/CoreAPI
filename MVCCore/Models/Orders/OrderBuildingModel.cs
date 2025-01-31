using Newtonsoft.Json;
using System;
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
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("count")]
        [Range(1, int.MaxValue, ErrorMessage = "Count must be greater than 0")]
        public int Count { get; set; }
    }
}
