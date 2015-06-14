using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Repositories;
using Cloud.Storages.Resources;
using DropboxRestAPI;

namespace Cloud.Storages.Dropbox {
	internal class Dropbox : IStorage {
		private readonly int _id;
		private readonly DropboxUserTokenRepository _tokenRepository;
		private readonly UserStoragesRepository _userStoragesRepository;
		
		private readonly DropboxManager _manager;

		public Dropbox(int id) {
			_id = id;
			_manager = new DropboxManager();
			_tokenRepository = new DropboxUserTokenRepository();
			_userStoragesRepository = new UserStoragesRepository();
		}

		#region IStorage implementation

		public async Task AuthorizeAsync(string userId, string code) {
			var options = new Options {
				ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
				ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
				RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
			};

			var client = new Client(options);
			var token = await client.Core.OAuth2.TokenAsync(code);
			var dropboxToken = new DropboxUserToken {
				UserId = userId,
				AccessToken = token.access_token,
				TeamId = token.team_id,
				TokenType = token.token_type,
				Uid = token.uid
			};

			await _tokenRepository.AddOrUpdateAsunc(dropboxToken, userId);
			await _userStoragesRepository.AddAsync(userId, _id);
		}

		public async Task<IFile> AddFileAsync( string userId, FullUserFile file ) {
			throw new NotImplementedException();
		}

		public async Task<IFolder> AddFolderAsync( string userId, IFolder folder ) {
			throw new NotImplementedException();
		}

		public async Task<FolderData> GetRootFolderDataAsync( string userId ) {
			var client = await _manager.GetClient(userId);
			var rootFilesFolders = await client.Core.Metadata.MetadataAsync(
				DropboxKeys.RootFolderPath);
			var folders = new List<IFolder>();
			var files = new List<IFile>();
			var folderData = new FolderData { StorageId = _id };
			foreach (var folderFile in rootFilesFolders.contents) {
				if (folderFile.is_dir) {
					folders.Add(new UserFolder {
						StorageId = _id,
						Id = _manager.ConstructEntityId(folderFile.path),
						UserId = userId,
						Name = folderFile.Name
					});
				} else {
					files.Add(new UserFile {
						StorageId = _id,
						Name = folderFile.Name,
						Id = _manager.ConstructEntityId(folderFile.path),
						UserId = userId
					});
				}
			}
			var folder = new UserFolder {
				Id = _manager.ConstructEntityId(DropboxKeys.RootFolderPath),
				StorageId = _id
			};
			folderData.Folders = folders;
			folderData.Files = files;
			folderData.Folder = folder;

			return folderData;
		}

		public async Task<FolderData> GetFolderDataAsync( string userId, string folderId ) {
			var client = await _manager.GetClient(userId);
			var filesFolders = await client.Core.Metadata.MetadataAsync(
				_manager.ConstructEntityPath(folderId));
			var folders = new List<IFolder>();
			var files = new List<IFile>();
			var folderData = new FolderData {StorageId = _id};
			foreach (var folderFile in filesFolders.contents) {
				if (folderFile.is_dir) {
					folders.Add(new UserFolder {
						StorageId = _id,
						Id = _manager.ConstructEntityId(folderFile.path),
						UserId = userId,
						Name = folderFile.Name,
					});
				} else {
					files.Add(new UserFile {
						StorageId = _id,
						Name = folderFile.Name,
						Id = _manager.ConstructEntityId(folderFile.path),
						UserId = userId,
					});
				}
			}
			var folder = new UserFolder {
				Id = _manager.ConstructEntityId(folderId),
				StorageId = _id
			};
			folderData.Folders = folders;
			folderData.Files = files;
			folderData.Folder = folder;

			return folderData;
		}

		public async Task<IFile> GetFileInfoAsync( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public async Task<FullUserFile> GetFileAsync( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public async Task<string> UpdateFileNameAsync( string userId, string fileId, string newfileName ) {
			var client = await _manager.GetClient(userId);
			var oldfilePathWithName = _manager.ConstructEntityPath(fileId);
			var fileExtention = Path.GetExtension(oldfilePathWithName);
			var oldfilePath = Path.GetDirectoryName(oldfilePathWithName);
			if (string.IsNullOrEmpty(oldfilePath)) {
				// todo:
				throw new Exception("todo");
			}

			var newFilePathWithName = _manager.MakeValidPath(
				Path.Combine(oldfilePath, string.Concat(newfileName, fileExtention)));
			var responce = await client.Core.FileOperations.MoveAsync(oldfilePathWithName, newFilePathWithName);

			// todo: validation
			//if (responce) {

			//}
			// todo:
			return responce.Name;
		}

		public async Task<string> UpdateFolderNameAsync( string userId, string folderId, string newFolderName ) {
			var client = await _manager.GetClient(userId);
			var oldFolderPathWithName = _manager.ConstructEntityPath(folderId);
			var oldfilePath = Path.GetDirectoryName(oldFolderPathWithName);
			if (string.IsNullOrEmpty(oldfilePath)) {
				// todo:
				throw new Exception("todo");
			}

			var newFilePathWithName = _manager.MakeValidPath(
				Path.Combine(oldfilePath, newFolderName));
			var responce = await client.Core.FileOperations.MoveAsync(oldFolderPathWithName, newFilePathWithName);

			// todo: validation
			//if (responce) {

			//}

			return responce.Name;
		}

		public async Task<bool> DeleteFileAsync( string userId, string fileId ) {
			var client = await _manager.GetClient(userId);
			var response = await client.Core.FileOperations.DeleteAsync(
				_manager.ConstructEntityPath(fileId));
			if (response.is_deleted == false) {
				// todo:
				throw new Exception("todo");
			}

			return true;
		}

		public async Task<bool> DeleteFolderAsync( string userId, string folderId ) {
			var client = await _manager.GetClient(userId);
			var response = await client.Core.FileOperations.DeleteAsync(
				_manager.ConstructEntityPath(folderId));
			if (response.is_deleted == false) {
				// todo:
				throw new Exception("todo");
			}

			return true;
		}

		#endregion IStorage implementation
	}
}