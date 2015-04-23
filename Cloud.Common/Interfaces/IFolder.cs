using Newtonsoft.Json;

namespace Cloud.Common.Interfaces
{
    public interface IFolder
    {
        [JsonProperty("id")]
        string Id { get; set; }

        [JsonProperty("name")]
        string Name { get; set; }
    }
}
