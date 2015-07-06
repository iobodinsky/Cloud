using Cloud.Common.Interfaces;

namespace Cloud.Common.Models {
	public class UserFolder : IFolder {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public string UserId { get; set; }

        public int StorageId { get; set; }
	}
}