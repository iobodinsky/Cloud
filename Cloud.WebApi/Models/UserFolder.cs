using Newtonsoft.Json;

namespace Cloud.WebApi.Models
{
    public class UserFolder
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}