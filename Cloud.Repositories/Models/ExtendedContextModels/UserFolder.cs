using Cloud.Common.Interfaces;

namespace Cloud.Repositories.DataContext {
	public partial class UserFolder : IFolder {
		public int StorageId { get; set; }
	}
}