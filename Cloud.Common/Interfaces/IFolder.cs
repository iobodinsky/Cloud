using Newtonsoft.Json;

namespace Cloud.Common.Interfaces {
	public interface IFolder {
		[JsonProperty( "id" )]
		string Id { get; set; }

		[JsonProperty( "name" )]
		string Name { get; set; }

		[JsonProperty( "parentId" )]
		string ParentId { get; set; }

		[JsonProperty( "userId" )]
		string UserId { get; set; }

		[JsonProperty("cloudId")]
		int CloudId { get; set; }
	}
}
