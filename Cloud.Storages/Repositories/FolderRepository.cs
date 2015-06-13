using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.Storages.LocalLenevo;

namespace Cloud.Storages.Repositories {
	public class FolderRepository : RepositoryBase {

		#region IFileRepository implementation

		public void Add( string userId, int cloudId, IFolder folder ) {
			throw new NotImplementedException();
		}

		public IFolder GetFolder( string userId, int cloudId, string folderId ) {
			throw new NotImplementedException();
		}

		public async Task<FolderData> GetFolderData( string userId, int cloudId, string folderId ) {
			var cloud = ResolveStorageInstance(cloudId);

			return await cloud.GetFolderDataAsync(userId, folderId);
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
			var serverManager = new FileServerManager();
			var folder = Entities.UserFolders
				.SingleOrDefault(folderItem => folderItem.Id == folderId);
			if (folder == null) {
				// todo: 
				throw new Exception("todo");
			}
			serverManager.DeleteFolder(userId, folder.Id);

			// Delete folder from Db
			Entities.UserFolders.Attach(folder);
			Entities.UserFolders.Remove(folder);
			SaveChanges();
		}

		#endregion IFileRepository implementation
	}
}