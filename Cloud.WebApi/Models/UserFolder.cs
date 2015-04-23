using Cloud.Common.Interfaces;

namespace Cloud.WebApi.Models
{
    public class UserFolder : IFolder
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}