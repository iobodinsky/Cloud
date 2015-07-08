using Newtonsoft.Json;

namespace Cloud.WebApi.Models
{
    public class ServerError
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("stackTrace")]
        public string StackTrace { get; set; }

        [JsonProperty("innerServerError")]
        public ServerError InnerServerError { get; set; }
    }
}