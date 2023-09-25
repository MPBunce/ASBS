using Newtonsoft.Json;

namespace webapi.Models
{

    public class Physiotherapist
    {
        [JsonProperty("id")]
        public string PhysiotherapistId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("contactNumber")]
        public string ContactNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("specialization")]
        public string Specialization { get; set; }
    }

}
