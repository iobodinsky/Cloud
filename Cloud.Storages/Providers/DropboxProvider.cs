using System.Collections.Generic;
using System.Configuration;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;
using Cloud.Storages.Resources;
using DropboxRestAPI;

namespace Cloud.Storages.Providers {
	internal class DropboxProvider : IStorage {

		private readonly DropboxManager _manager;

		public DropboxProvider() {
			_manager = new DropboxManager();
		}

		#region IStorage implementation

		public void Authorize() {
			throw new System.NotImplementedException();
		}

		public void AddFile( string userId, FullUserFile file ) {
			throw new System.NotImplementedException();
		}

		public void AddFolder( string userId, IFolder folder ) {
			throw new System.NotImplementedException();
		}

		public FolderData GetRootFolderData( string userId ) {
			var client = _manager.GetClient().Result;
			var rootFilesFolders = client.Core.Metadata.MetadataAsync("/").Result;
			var folders = new List<IFolder>();
			var files = new List<IFile>();
			var folderData = new FolderData { CloudId = 3 };
			foreach (var folderFile in rootFilesFolders.contents) {
				if (folderFile.is_dir) {
					folders.Add(new UserFolder {
						CloudId = 3,
						Id = folderFile.Name,
						//ParentId = "",Name = folder.Name,
						UserId = userId,
						Name = folderFile.Name
					});
				} else {
					files.Add(new UserFile {
						CloudId = 3,
						Name = folderFile.Name,
						Id = folderFile.Name,
						UserId = userId,
						//Size = folderFile.size,
					});
				}
			}
			folderData.Folders = folders;
			folderData.Files = files;

			return folderData;
		}

		public FolderData GetFolderData( string userId, string folderId ) {
			throw new System.NotImplementedException();
		}

		public IFile GetFileInfo( string userId, string fileId ) {
			throw new System.NotImplementedException();
		}

		public FullUserFile GetFile( string userId, string fileId ) {
			throw new System.NotImplementedException();
		}

		public void UpdateName( string userId, string fileId, string newfileName ) {
			throw new System.NotImplementedException();
		}

		public void DeleteFile( string userId, string fileId ) {
			throw new System.NotImplementedException();
		}

		public void DeleteFolder( string userId, string folderId ) {
			throw new System.NotImplementedException();
		}

		#endregion IStorage implementation
	}
}
