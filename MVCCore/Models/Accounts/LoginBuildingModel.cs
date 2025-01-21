using Newtonsoft.Json;

namespace MVCCore.Models.Accounts
{
    public class LoginBuildingModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
