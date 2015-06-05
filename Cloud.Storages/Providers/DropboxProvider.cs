using System;
using System.Collections.Generic;
using System.IO;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;

namespace Cloud.Storages.Providers {
	internal class DropboxProvider : IStorage {

		private readonly DropboxManager _manager;

		public DropboxProvider() {
			_manager = new DropboxManager();
		}

		#region IStorage implementation

		public void Authorize() {
			throw new NotImplementedException();
		}

		public void AddFile( string userId, FullUserFile file ) {
			throw new NotImplementedException();
		}

		public void AddFolder( string userId, IFolder folder ) {
			throw new NotImplementedException();
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
						Id =  _manager.ConstructFileId(folderFile.path),
						//ParentId = "",Name = folder.Name,
						UserId = userId,
						Name = folderFile.Name,
					});
				} else {
					files.Add(new UserFile {
						CloudId = 3,
						Name = folderFile.Name,
						Id = _manager.ConstructFileId(folderFile.path),
						UserId = userId,
						//Size = folderFile.size
					});
				}
			}
			folderData.Folders = folders;
			folderData.Files = files;

			return folderData;
		}

		public FolderData GetFolderData( string userId, string folderId ) {
			throw new NotImplementedException();
		}

		public IFile GetFileInfo( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public FullUserFile GetFile( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public void UpdateName( string userId, string fileId, string newfileName ) {
			var client = _manager.GetClient().Result;
			var oldfilePathWithName = _manager.ConstructFilePath(fileId);
			var fileExtention = Path.GetExtension(oldfilePathWithName);
			var oldfilePath = Path.GetDirectoryName(oldfilePathWithName);
			if (string.IsNullOrEmpty(oldfilePath)) {
				// todo:
				throw new Exception("todo");
			}

			var newFilePathWithName = _manager.MakeValidPath(
				Path.Combine(oldfilePath, string.Concat(newfileName, fileExtention)));
			var responce = client.Core.FileOperations.MoveAsync(oldfilePathWithName, newFilePathWithName);

			// todo: validation
			//if (responce) {
				
			//}
		}

		public void DeleteFile( string userId, string fileId ) {
			var client = _manager.GetClient().Result;
			var response = client.Core.FileOperations.DeleteAsync(
				_manager.ConstructFilePath(fileId)).Result;
			if (response.is_deleted == false) {
				// todo:
				throw new Exception("todo");
			}
		}

		public void DeleteFolder( string userId, string folderId ) {
			var client = _manager.GetClient().Result;
			var response = client.Core.FileOperations.DeleteAsync(
				_manager.ConstructFilePath(folderId)).Result;
			if (response.is_deleted == false) {
				// todo:
				throw new Exception("todo");
			}
		}

		#endregion IStorage implementation
	}
}
