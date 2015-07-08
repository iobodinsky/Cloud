using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Newtonsoft.Json;

namespace Cloud.Common.Models
{
    public class FolderData
    {
        [JsonProperty("folders")]
        public IEnumerable<IFolder> Folders { get; set; }

        [JsonProperty("files")]
        public IEnumerable<IFile> Files { get; set; }

        [JsonProperty("folder")]
        public IFolder Folder { get; set; }

        [JsonProperty("storageId")]
        public int StorageId { get; set; }
    }
}