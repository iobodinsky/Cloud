using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cloud.WebApi.Models
{
    public class UserStorage
    {
        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; set; }

        [JsonProperty("files")]
        public IEnumerable<UserFileInfo> Files { get; set; }
    }
}