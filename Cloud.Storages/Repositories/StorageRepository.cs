using System;
using System.Collections.Generic;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Resources;
using Cloud.Storages.Managers;

namespace Cloud.Storages.Repositories {
	public class StorageRepository : RepositoryBase {

		#region IFileRepository implementation

		public IFile GetFileInfo( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public IFile Get( string userId, string fileId ) {
			return Entities.UserFiles.SingleOrDefault(
				file => file.UserId == userId && file.Id == fileId);
		}

		public IEnumerable<IFolder> GetRootFolders( string userId ) {
			var rootFolderId = GetUserRootFolderId(userId);
			return Entities.UserFolders
				.Where(folder => folder.UserId == userId &&
				                 folder.ParentId == rootFolderId);
		}

		public IFolder GetRootFolder( string userId ) {
			var userRootFolderId = GetUserRootFolderId(userId);
			var folder = Entities.UserFolders
				.SingleOrDefault(folderItem => folderItem.Id == userRootFolderId);
			return folder;
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