using Newtonsoft.Json;

namespace Cloud.WebApi.Models {
	public class NewNameModel {
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}