using Newtonsoft.Json;

namespace Cloud.WebApi.Models {
	public class NewFileModel {
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}