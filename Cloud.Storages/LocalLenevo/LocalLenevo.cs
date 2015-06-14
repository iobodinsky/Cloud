﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Common.Resources;
using Cloud.Repositories;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Repositories;

namespace Cloud.Storages.LocalLenevo {
	internal class LocalLenevo : IStorage {
		#region Private fields

		private readonly int _id;
		private readonly StorageRepository _storageRepository;
		private readonly UserStoragesRepository _userStoragesRepository;
		private readonly LocalLenovoManager _fileServerManager;

		#endregion Private fields

		public LocalLenevo( int id ) {
			_id = id;
			_fileServerManager = new LocalLenovoManager();
			_userStoragesRepository = new UserStoragesRepository();
			_storageRepository = new StorageRepository();
		}

		#region IStorage implementation

		public async Task AuthorizeAsync( string userId, string code ) {
			// Create user root directory on servers
			var userRootFolder = new UserFolder {
				Id = userId,
				Name = userId,
				ParentId = LocalCloudCommon.RootFolderId,
				UserId = userId
			};

			var cloud = new StorageRepository().ResolveStorageInstance(
				Constants.LocalLenovoStorageId);
			await cloud.AddFolderAsync(userId, userRootFolder);

			await _userStoragesRepository.AddAsync(
				userId, Constants.LocalLenovoStorageId);
		}

		public async Task DisconnectAsync( string userId ) {
			await _userStoragesRepository.DeleteAsync(userId, _id);
		}

		public async Task<IFile> AddFileAsync( string userId, FullUserFile file ) {
			await Task.Run(() => _fileServerManager.AddFile(userId, file));

			// todo: try catch (in catch undo) for undo if db failed
			// Save file info to Db
			await _storageRepository.AddAsync(file.UserFile as UserFile, true);

			return file.UserFile;
		}

		public async Task<IFolder> AddFolderAsync( string userId, IFolder folder ) {
			// Save folder on all physical servers
			_fileServerManager.AddFolder(userId, folder);

			// todo: try catch for undo if db failed
			// todo: entity as UserFolder
			// Save file info to Db
			if (_storageRepository.Entities.UserFolders
				.Any(folderItem => folderItem.UserId == userId &&
				                   folderItem.Name == folder.Name &&
				                   folderItem.ParentId == folder.ParentId)) return null;

			await _storageRepository.AddAsync(folder as UserFolder, true);

			return folder;
		}

		public async Task<FolderData> GetRootFolderDataAsync( string userId ) {
			var rootFolderId = _fileServerManager.GetUserRootFolderId(userId);
			var folders = _storageRepository.Entities.UserFolders
				.Where(folder => folder.UserId == userId &&
				                 folder.ParentId == rootFolderId).ToList();
			var files = _storageRepository.Entities.UserFiles
				.Where(file => file.UserId == userId &&
				               file.FolderId == rootFolderId).ToList();
			var currentFolder = _storageRepository.Entities.UserFolders
				.SingleOrDefault(folder => folder.UserId == userId &&
				                           folder.Id == rootFolderId);
			if (currentFolder == null) {
				// todo: 
				throw new Exception("todo");
			}
			currentFolder.StorageId = _id;
			foreach (var folder in folders) {
				folder.StorageId = _id;
			}
			foreach (var file in files) {
				file.StorageId = _id;
			}
			var folderData = new FolderData {
				Folders = folders,
				Files = files,
				Folder = currentFolder,
				StorageId = _id
			};

			return folderData;
		}

		public async Task<FolderData> GetFolderDataAsync( string userId, string folderId ) {
			var folders = _storageRepository.Entities.UserFolders
				.Where(folder => folder.UserId == userId &&
				                 folder.ParentId == folderId).ToList();
			var files = _storageRepository.Entities.UserFiles
				.Where(file => file.UserId == userId &&
				               file.FolderId == folderId).ToList();
			var currentFolder = _storageRepository.Entities.UserFolders
				.SingleOrDefault(folder => folder.UserId == userId &&
				                           folder.Id == folderId);
			if (currentFolder == null) {
				// todo: 
				throw new Exception("todo");
			}
			currentFolder.StorageId = _id;
			foreach (var folder in folders) {
				folder.StorageId = _id;
			}
			foreach (var file in files) {
				file.StorageId = _id;
			}
			var folderData = new FolderData {
				Folders = folders,
				Files = files,
				Folder = currentFolder
			};

			return folderData;
		}

		public async Task<IFile> GetFileInfoAsync( string userId, string fileId ) {
			return _storageRepository.GetFileInfo(userId, fileId);
		}

		public async Task<FullUserFile> GetFileAsync( string userId, string fileId ) {
			return await Task.Run(() => _fileServerManager.GetFile(userId, fileId));
		}

		public async Task<string> UpdateFileNameAsync( string userId, string fileId, string newfileName ) {
			var fileToUpdate = _storageRepository.Entities.UserFiles
				.SingleOrDefault(file => file.Id == fileId && file.UserId == userId);
			if (fileToUpdate == null) {
				// todo
				throw new Exception("todo");
			}

			// Rename file on servers
			var oldfileName = fileToUpdate.Name;
			var extention = Path.GetExtension(oldfileName);
			newfileName = Path.GetFileNameWithoutExtension(newfileName);
			newfileName += extention;
			var serverManager = new LocalLenovoManager();
			serverManager.RenameFile(userId, fileToUpdate, newfileName);

			// Rename file in Db
			fileToUpdate.Name = newfileName;
			_storageRepository.Entities.UserFiles.Attach(fileToUpdate);
			var entry = _storageRepository.Entities.Entry(fileToUpdate);
			entry.Property(file => file.Name).IsModified = true;
			_storageRepository.SaveChanges();

			return newfileName;
		}

		public async Task<string> UpdateFolderNameAsync( string userId, string folderId, string newFolderName ) {
			await Task.Run(() => {
				var folderToUpdate = _storageRepository.Entities.UserFolders
					.SingleOrDefault(folder => folder.Id == folderId && folder.UserId == userId);
				if (folderToUpdate == null) {
					// todo
					throw new Exception("todo");
				}

				// Rename folder on servers
				var oldFolderName = folderToUpdate.Name;
				var serverManager = new LocalLenovoManager();
				serverManager.RenameFolder(userId, folderId, oldFolderName, newFolderName);

				// Rename folder in Db
				folderToUpdate.Name = newFolderName;
				_storageRepository.Entities.UserFolders.Attach(folderToUpdate);
				var entry = _storageRepository.Entities.Entry(folderToUpdate);
				entry.Property(file => file.Name).IsModified = true;
				_storageRepository.SaveChanges();
			});

			return newFolderName;
		}

		public async Task DeleteFileAsync( string userId, string fileId ) {
			// Delete file from servers
			var file = _storageRepository.Entities.UserFiles
				.SingleOrDefault(fileItem => fileItem.UserId == userId &&
				                             fileItem.Id == fileId);
			if (file == null) {
				// todo:
				throw new Exception("todo");
			}

			await Task.Run(() => _fileServerManager.DeleteFile(userId, file.Id));

			await Task.Run(() => {
				// Delete file from Db
				_storageRepository.Entities.UserFiles.Attach(file);
				_storageRepository.Entities.UserFiles.Remove(file);
				_storageRepository.SaveChanges();
			});
		}

		public async Task DeleteFolderAsync( string userId, string folderId ) {
			var folder = _storageRepository.Entities.UserFolders
				.SingleOrDefault(folderItem => folderItem.UserId == userId &&
				                               folderItem.Id == folderId);
			if (folder == null) {
				// todo: 
				throw new Exception("todo");
			}

			// Delete folder from servers
			await Task.Run(() => _fileServerManager.DeleteFolder(userId, folder.Id));

			// todo: remove all subfolders and subfiles
			// Delete folder, subfolders and subfiles from Db
			await Task.Run(() => {
				_storageRepository.Entities.UserFolders.Attach(folder);
				_storageRepository.Entities.UserFolders.Remove(folder);
				_storageRepository.SaveChanges();
			});
		}

		#endregion IStorage implementation
	}
}