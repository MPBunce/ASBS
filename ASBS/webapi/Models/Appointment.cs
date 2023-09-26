using Newtonsoft.Json;

namespace webapi.Models
{
    public class Appointment
    {
        [JsonProperty("appointmentId")]
        public string AppointmentId { get; set; }

        [JsonProperty("physiotherapist")]
        public Physiotherapist Physiotherapist { get; set; }

        [JsonProperty("appointmentDateTime")]
        public String AppointmentDateTime { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}
