using Newtonsoft.Json;

namespace Cloud.WebApi.Models
{
    public class UserFile
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}