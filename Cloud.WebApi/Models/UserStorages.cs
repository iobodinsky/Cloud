using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cloud.WebApi.Models
{
    public class UserStorages
    {
        [JsonProperty("connected")]
        public IEnumerable<StorageModel> Connected { get; set; }

        [JsonProperty("available")]
        public IEnumerable<StorageModel> Available { get; set; }
    }
}