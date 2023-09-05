using Newtonsoft.Json;

namespace webapi.Models
{
    public class Patient
    {

        [JsonProperty("id")]
        public string? PatientId { get; set;}

        [JsonProperty("firstname")]
        public string FirstName { get; set;}

        [JsonProperty("lastname")]
        public string LastName { get; set;}

        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set;}

        [JsonProperty("email")]
        public string Email { get; set;}

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("appointments")]
        public List<Appointment>? Appointments { get; set; }

    }

}
