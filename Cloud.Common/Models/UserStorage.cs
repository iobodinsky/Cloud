using System.Collections.Generic;
using Cloud.WebApi.Models;
using Newtonsoft.Json;

namespace Cloud.Common.Models
{
    public class UserStorage
    {
        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; set; }

        [JsonProperty("files")]
        public IEnumerable<UserFile> Files { get; set; }
    }
}