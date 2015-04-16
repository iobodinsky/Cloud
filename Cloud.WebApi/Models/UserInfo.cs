using Newtonsoft.Json;

namespace Cloud.WebApi.Models
{
    public class UserInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}