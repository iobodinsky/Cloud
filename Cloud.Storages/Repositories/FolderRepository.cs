using System.Collections.Generic;
using Cloud.Common.Interfaces;
namespace Cloud.Storages.Repositories {
	public class FolderRepository : RepositoryBase, IFolderRepository {

		#region IFileRepository implementation

		public void Add( string userId, int cloudId, IFolder folder ) {
			var cloud = ResolveStorageInstance(cloudId);
			cloud.AddFolder(userId, folder);
		}

		public IFolder GetFolder( string userId, int cloudId, string folderId ) {
			throw new System.NotImplementedException();
		}

		public IEnumerable<IFile> GetRootFiles( string userId ) {
			throw new System.NotImplementedException();
		}

		public IEnumerable<IFolder> GetRootFolders( string userId ) {
			throw new System.NotImplementedException();
		}

		public string GetRootFolderId( string userId ) {
			throw new System.NotImplementedException();
		}

		public void UpdateName( string userId, int cloudId, string folderId, string newfolderName ) {
			throw new System.NotImplementedException();
		}

		public void Delete( string userId, int cloudId, string folderId ) {
			throw new System.NotImplementedException();
		}

		#endregion IFileRepository implementation
	}
}
