using System.Collections.Generic;
using System.Runtime.Serialization;
using Cloud.Common.Interfaces;
using Newtonsoft.Json;

namespace Cloud.WebApi.Models {
	[CollectionDataContract]
	public class FoldersFiles {
		[JsonProperty("folders")]
		public IEnumerable<IFolder> Folders { get; set; }

		[JsonProperty("files")]
		public IEnumerable<IFile> Files { get; set; }

		[JsonProperty("currentFolderId")]
		public string CurrentFolderId { get; set; }
	}
}