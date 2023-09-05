using Newtonsoft.Json;

namespace webapi.Models
{
    public class Auth
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
