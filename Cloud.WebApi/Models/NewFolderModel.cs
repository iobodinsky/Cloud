using Newtonsoft.Json;

namespace Cloud.WebApi.Models {
	public class NewFolderModel {
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}