using Newtonsoft.Json;

namespace webapi.Models
{

    public class Physiotherapist
    {
        [JsonProperty("physiotherapistId")]
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

        [JsonProperty("workingDays")]
        public string WorkingDays { get; set; }

        [JsonProperty("workingHoursStart")]
        public string WorkingHoursStart { get; set; }

        [JsonProperty("workingHoursEnd")]
        public string WorkingHoursEnd { get; set; }
    }

}
