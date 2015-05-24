using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;
using Cloud.Storages.Repositories;

namespace Cloud.Storages.Providers {
	internal class LocalLenevoProvider : IStorage {
		#region Private fields

		private readonly StorageRepository _storageRepository;
		private readonly FileServerManager _fileServerManager;

		#endregion Private fields

		public LocalLenevoProvider() {
			_fileServerManager = new FileServerManager();
			_storageRepository = new StorageRepository();
		}

		#region IStorage implementation

		public void AddFile( string userId, FullUserFile file ) {
			// Save file on all servers
			_fileServerManager.AddFile(userId, file);

			// todo: try catch (in catch undo) for undo if db failed
			// Save file info to Db
			_storageRepository.Add(file.UserFile as UserFile, true);
		}

		public void AddFolder( string userId, IFolder folder ) {
			// Save folder on all physical servers
			_fileServerManager.AddFolder(userId, folder, true);

			// todo: try catch for undo if db failed
			// todo: entity as UserFolder
			// Save file info to Db
			_storageRepository.Add(folder as UserFolder, true);
		}

		public IEnumerable<IFile> GetRootFiles( string userId ) {
			var rootFolderId = _fileServerManager.GetUserRootFolderId(userId);
			return _storageRepository.Entities.UserFiles
				.Where(file => file.UserId == userId && file.FolderId == rootFolderId);
		}

		public IEnumerable<IFolder> GetRootFolders( string userId ) {
			var rootFolderId = _fileServerManager.GetUserRootFolderId(userId);
			return _storageRepository.Entities.UserFolders
				.Where(folder => folder.UserId == userId &&
				                 folder.ParentId == rootFolderId);
		}

		public FolderData GetFolderData( string userId, string folderId ) {
			var folders = _storageRepository.Entities.UserFolders
				.Where(folder => folder.UserId == userId &&
									  folder.ParentId == folderId);
			var files = _storageRepository.Entities.UserFiles
				.Where(file => file.UserId == userId &&
									file.FolderId == folderId);
			var currentFolder = _storageRepository.Entities.UserFolders
				.SingleOrDefault(folder => folder.UserId == userId &&
													folder.Id == folderId);
			var folderData = new FolderData {
				Folders = folders,
				Files = files,
				Folder = currentFolder
			};

			return folderData;
		}

		public IFile GetFileInfo( string userId, string fileId ) {
			return _storageRepository.GetFileInfo(userId, fileId);
		}

		public FullUserFile GetFile( string userId, string fileId ) {
			return _fileServerManager.GetFile(userId, fileId);
		}

		public void UpdateName( string userId, string fileId, string newfileName ) {
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
			var serverManager = new FileServerManager();
			serverManager.RenameFile(userId, fileToUpdate, newfileName);
			
			// Rename file in Db
			fileToUpdate.Name = newfileName;
			_storageRepository.Entities.UserFiles.Attach(fileToUpdate);
			var entry = _storageRepository.Entities.Entry(fileToUpdate);
			entry.Property(file => file.Name).IsModified = true;
			_storageRepository.SaveChanges();
		}

		public void DeleteFile(string userId, string fileId) {
			throw new NotImplementedException();
		}

		public void DeleteFolder(string userId, string folderId) {
			_fileServerManager.DeleteFolder(userId, folderId);
		}

		#endregion IStorage implementation
	}
}
