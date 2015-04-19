using System;
using Newtonsoft.Json;

namespace Cloud.WebApi.Models
{
    public class UserFileDetails : UserFileInfo
    {
        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }

        [JsonProperty("addedDateTime")]
        public DateTime AddedDateTime { get; set; }

        [JsonProperty("downloadedTimes")]
        public int DownloadedTimes { get; set; }
    }
}