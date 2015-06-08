using System;
using System.Collections.Generic;
using System.IO;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;
using Cloud.Storages.Resources;

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
			var rootFilesFolders = client.Core.Metadata.MetadataAsync(
				DropboxKeys.RootFolderPath).Result;
			var folders = new List<IFolder>();
			var files = new List<IFile>();
			var folderData = new FolderData { CloudId = 3 };
			foreach (var folderFile in rootFilesFolders.contents) {
				if (folderFile.is_dir) {
					folders.Add(new UserFolder {
						CloudId = 3,
						Id =  _manager.ConstructEntityId(folderFile.path),
						UserId = userId,
						Name = folderFile.Name
					});
				} else {
					files.Add(new UserFile {
						CloudId = 3,
						Name = folderFile.Name,
						Id = _manager.ConstructEntityId(folderFile.path),
						UserId = userId
					});
				}
			}
			var folder = new UserFolder {
				Id = _manager.ConstructEntityId(DropboxKeys.RootFolderPath),
				CloudId = 3
			};
			folderData.Folders = folders;
			folderData.Files = files;
			folderData.Folder = folder;

			return folderData;
		}

		public FolderData GetFolderData( string userId, string folderId ) {
			var client = _manager.GetClient().Result;
			var filesFolders = client.Core.Metadata.MetadataAsync(
				_manager.ConstructEntityPath(folderId)).Result;
			var folders = new List<IFolder>();
			var files = new List<IFile>();
			var folderData = new FolderData { CloudId = 3 };
			foreach (var folderFile in filesFolders.contents) {
				if (folderFile.is_dir) {
					folders.Add(new UserFolder {
						CloudId = 3,
						Id = _manager.ConstructEntityId(folderFile.path),
						UserId = userId,
						Name = folderFile.Name,
					});
				} else {
					files.Add(new UserFile {
						CloudId = 3,
						Name = folderFile.Name,
						Id = _manager.ConstructEntityId(folderFile.path),
						UserId = userId,
					});
				}
			}
			var folder = new UserFolder {
				Id = _manager.ConstructEntityId(folderId),
				CloudId = 3
			};
			folderData.Folders = folders;
			folderData.Files = files;
			folderData.Folder = folder;

			return folderData;
		}

		public IFile GetFileInfo( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public FullUserFile GetFile( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public void UpdateFileName(string userId, string fileId, string newfileName) {
			var client = _manager.GetClient().Result;
			var oldfilePathWithName = _manager.ConstructEntityPath(fileId);
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

		public void UpdateFolderName(string userId, string folderId, string newFolderName) {
			var client = _manager.GetClient().Result;
			var oldFolderPathWithName = _manager.ConstructEntityPath(folderId);
			var oldfilePath = Path.GetDirectoryName(oldFolderPathWithName);
			if (string.IsNullOrEmpty(oldfilePath)) {
				// todo:
				throw new Exception("todo");
			}

			var newFilePathWithName = _manager.MakeValidPath(
				Path.Combine(oldfilePath, newFolderName));
			var responce = client.Core.FileOperations.MoveAsync(oldFolderPathWithName, newFilePathWithName);

			// todo: validation
			//if (responce) {

			//}
		}

		public void DeleteFile( string userId, string fileId ) {
			var client = _manager.GetClient().Result;
			var response = client.Core.FileOperations.DeleteAsync(
				_manager.ConstructEntityPath(fileId)).Result;
			if (response.is_deleted == false) {
				// todo:
				throw new Exception("todo");
			}
		}

		public void DeleteFolder( string userId, string folderId ) {
			var client = _manager.GetClient().Result;
			var response = client.Core.FileOperations.DeleteAsync(
				_manager.ConstructEntityPath(folderId)).Result;
			if (response.is_deleted == false) {
				// todo:
				throw new Exception("todo");
			}
		}

		#endregion IStorage implementation
	}
}
