using System;
using System.Collections.Generic;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Storages.Managers;

namespace Cloud.Storages.Repositories {
	public class FolderRepository : RepositoryBase, IFolderRepository {

		#region IFileRepository implementation

		public void Add( string userId, int cloudId, IFolder folder ) {
			var cloud = ResolveStorageInstance(cloudId);
			cloud.AddFolder(userId, folder);
		}

		public IFolder GetFolder( string userId, int cloudId, string folderId ) {
			throw new NotImplementedException();
		}

		public IEnumerable<IFile> GetRootFiles( string userId ) {
			throw new NotImplementedException();
		}

		public IEnumerable<IFolder> GetRootFolders( string userId ) {
			throw new NotImplementedException();
		}

		public string GetRootFolderId( string userId ) {
			throw new NotImplementedException();
		}

		public void UpdateName( string userId, int cloudId, string folderId, string newfolderName ) {
			throw new NotImplementedException();
		}

		public void Delete( string userId, int cloudId, string folderId ) {
			// Delete folder on servers
			var serverManager = new LocalFileServerManager();
			var folder = Entities.UserFolders
				.SingleOrDefault(folderItem => folderItem.Id == folderId);
			if (folder == null) {
				// todo: 
				throw new Exception("todo");
			}			
			serverManager.DeleteFolder(userId, folder);

			// Delete folder from Db
			Entities.UserFolders.Attach(folder);
			Entities.UserFolders.Remove(folder);
			SaveChanges();
		}

		#endregion IFileRepository implementation
	}
}
