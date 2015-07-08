using Newtonsoft.Json;

namespace Cloud.Common.Models
{
    public class UserInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}