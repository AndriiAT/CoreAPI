using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MVCCore.Models.Accounts
{
    public class AccountBuildingModel
    {
        [Required]
        [EmailAddress]
        [JsonProperty("mail")]
        public string Mail { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
        
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
