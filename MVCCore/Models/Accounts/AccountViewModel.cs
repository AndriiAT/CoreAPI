using Newtonsoft.Json;
using System;

namespace MVCCore.Models.Accounts
{
    public class AccountViewModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("creation_date")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("role")]
        public string? Role { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }
    }
}
