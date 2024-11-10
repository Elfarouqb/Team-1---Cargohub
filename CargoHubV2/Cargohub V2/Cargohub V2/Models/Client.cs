using Cargohub_V2.DataConverters;
using Newtonsoft.Json;

namespace Cargohub_V2.Models
{
    public class Client
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("zip_code")]
        public string? ZipCode { get; set; }

        [JsonProperty("province")]
        public string? Province { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("contact_name")]
        public string? ContactName { get; set; }

        [JsonProperty("contact_phone")]
        public string? ContactPhone { get; set; }

        [JsonProperty("contact_email")]
        public string? ContactEmail { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(FlexibleDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        [JsonConverter(typeof(FlexibleDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }
    }
}
