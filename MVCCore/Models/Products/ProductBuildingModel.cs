using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MVCCore.Models.Products
{
    public class ProductBuildingModel
    {
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty("price")]
        [Required]
        public decimal Price { get; set; }

        [JsonProperty("Description")]
        [Required]
        public string Description { get; set; }
    }
}
