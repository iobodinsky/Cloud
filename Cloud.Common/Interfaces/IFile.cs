using System;
using Newtonsoft.Json;

namespace Cloud.Common.Interfaces {
	public interface IFile {
		[JsonProperty( "id" )]
		string Id { get; set; }

		[JsonProperty( "userId" )]
		string UserId { get; set; }

		[JsonProperty( "name" )]
		string Name { get; set; }

		[JsonProperty( "folderId" )]
		string FolderId { get; set; }

		[JsonProperty( "typeId" )]
		int? TypeId { get; set; }

		[JsonProperty( "isEditable" )]
		bool IsEditable { get; set; }

		[JsonProperty( "size" )]
		long Size { get; set; }

		[JsonProperty( "lastModifiedDateTime" )]
		DateTime LastModifiedDateTime { get; set; }

		[JsonProperty( "addedDateTime" )]
		DateTime AddedDateTime { get; set; }

		[JsonProperty( "downloadedTimes" )]
		int DownloadedTimes { get; set; }

		[JsonProperty( "downloadUrl" )]
		string DownloadUrl { get; set; }

		[JsonProperty( "cloudId" )]
		int CloudId { get; set; }
	}
}