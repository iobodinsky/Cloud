using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.WebApi.Models;
using Newtonsoft.Json;

namespace Cloud.Common.Models
{
    public class UserStorage
    {
        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; set; }

        [JsonProperty("files")]
        public IEnumerable<IFile> Files { get; set; }

        [JsonProperty("folders")]
        public IEnumerable<IFolder> Folders { get; set; }
    }
}