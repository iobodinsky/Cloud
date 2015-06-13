using Cloud.Common.Interfaces;

namespace Cloud.Repositories.DataContext {
	public partial class UserFile : IFile {
		public string DownloadUrl { get; set; }
		public int CloudId { get; set; }
	}
}