using System;
using System.Collections.Generic;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Resources;
using Cloud.Storages.Managers;
using Cloud.Storages.Providers;

namespace Cloud.Storages.Repositories {
	public class StorageRepository : RepositoryBase {
		#region Fields

		private readonly IList<IStorage> _storages;

		#endregion Fields

		public StorageRepository() {
			// todo: implement MEF
			_storages = new List<IStorage> {
				new LocalLenevoProvider(), 
				//new DriveProvider()
			};
		}

		#region IFileRepository implementation

		public IFile GetFileInfo( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public IFile Get( string userId, string fileId ) {
			return Entities.UserFiles.SingleOrDefault(
				file => file.UserId == userId && file.Id == fileId);
		}

		public IEnumerable<IFile> GetRootFiles( string userId ) {
			var files = new List<IFile>();
			foreach (var storage in _storages) {
				files.AddRange(storage.GetRootFiles(userId));
			}

			return files;
		}

		public IEnumerable<IFolder> GetRootFolders( string userId ) {
			var rootFolderId = GetUserRootFolderId(userId);
			return Entities.UserFolders
				.Where(folder => folder.UserId == userId &&
				                 folder.ParentId == rootFolderId);
		}

		public IFolder GetRootFolder( string userId ) {
			var serverManager = new FileServerManager();
			return serverManager.GetRootFolder(userId);
		}

		#endregion IFileRepository implementation

		#region Private methods

		private string GetUserRootFolderId(string userId) {
			var userRootFolder = Entities.UserFolders
				.SingleOrDefault(folder => folder.Id == userId);
			if (userRootFolder == null) throw new Exception(ErrorMessages.InvalidUserRootFolder);
			return userRootFolder.Id;
		}

		#endregion Private methods
	}
}