using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Common.Resources;
using Cloud.Storages.DataContext;
using Cloud.Storages.Repositories;

namespace Cloud.Storages.Storages.LocalLenevo {
	public class FileServerManager {
		#region Fields

		private readonly StorageRepository _storageRepository;

		#endregion Fields

		public FileServerManager() {
			_storageRepository = new StorageRepository();
		}

		#region Public methods

		public FullUserFile GetFile(string userId, string fileId) {
			var server = GetFileServers().First();
			var file = _storageRepository.Entities.UserFiles
				.SingleOrDefault(fileItem => fileItem.UserId == userId &&
					fileItem.Id == fileId);
			if (file == null) {
				// todo: 
				throw new Exception("todo");
			}

			var filePath = Path.Combine(
				GetFolderServerPath(userId, file.FolderId, server), file.Name);

			var fullFile = new FullUserFile {
				Stream = new FileStream(filePath, FileMode.Open),
				UserFile = file
			};

			return fullFile;
		}

		public void AddFile(string userId, FullUserFile file) {
			if (!HasUserEnoughFreeSpace()) {
				// todo:
				throw new Exception("todo");
			}

			foreach (var server in GetFileServers()) {
				var folderPath = GetFolderServerPath(userId, file.UserFile.FolderId, server);
				var filePath = Path.Combine(folderPath, file.UserFile.Name);
				if (File.Exists(filePath)) throw new Exception("File already exist");
				using (var createdFileStream = File.Open(filePath, FileMode.CreateNew)) {
					// Save the same file stream on all servers
					file.Stream.Position = 0;
					file.Stream.CopyTo(createdFileStream);
				}
			}
		}

		public void AddFolder( string userId, IFolder folder ) {
			var servers = GetFileServers();
			if (!servers.Any()) {
				// todo:
				throw new Exception("todo");
			}
			string folderPath;
			
			if (folder.ParentId.Equals(LocalCloudCommon.RootFolderId)) {
				// For creating root folder
				folderPath = userId;
			} else {
				folderPath = Path.Combine(
					GetFolderPath(userId, folder.ParentId), folder.Name);
			}
			
			foreach (var server in servers) {
				if (Directory.Exists(Path.Combine(server.Path, folderPath))) {
					// todo:
					throw new Exception("todo");
				}

				Directory.CreateDirectory(Path.Combine(server.Path, folderPath));
			}
		}

		public void RenameFile( string userId, IFile file, string newFileName ) {
			foreach (var server in GetFileServers()) {
				var folderPath = GetFolderServerPath(userId, file.FolderId, server);
				var oldFilePath = Path.Combine(folderPath, file.Name);
				var newFilePath = Path.Combine(folderPath, newFileName);
				if (File.Exists(newFilePath)) {
					// todo:
					throw new Exception("todo");
				}

				File.Move(oldFilePath, newFilePath);
			}
		}

		public void RenameFolder(string userId, string folderId, string oldFolderName, string newFolderName) {
			foreach (var server in GetFileServers()) {
				var oldFolderPath = GetFolderServerPath(userId, folderId, server);
				var newFolderPath = Path.Combine(
					Directory.GetParent(oldFolderPath).FullName, newFolderName);
				if (Directory.Exists(newFolderPath)) {
					// todo:
					throw new Exception("todo");
				}

				Directory.Move(oldFolderPath, newFolderPath);
			}
		}

		public void DeleteFile( string userId, string fileId ) {
			foreach (var server in GetFileServers()) {
				var filePath = GetFileServerPath(userId, fileId, server);
				if (!File.Exists(filePath)) {
					// todo:
					throw new Exception("todo");
				}
				File.Delete(filePath);
			}
		}

		public void DeleteFolder( string userId, string folderId ) {
			foreach (var server in GetFileServers()) {
				var folderPath = GetFolderServerPath(userId, folderId, server);
				if (Directory.Exists(folderPath)) {
					Directory.Delete(folderPath, true);
				}
			}
		}

		public string GetUserRootFolderId(string userId) {
			return userId;
		}

		#endregion Public methods

		#region Private methods

		private string GetFileServerPath( string userId, string fileId,
			LocalFileServer server ) {
			return Path.Combine(server.Path, GetFilePath(userId, fileId));
		}

		private string GetFilePath( string userId, string fileId ) {
			var file = _storageRepository.Entities.UserFiles
				.SingleOrDefault(fileItem => fileItem.Id == fileId);
			if (file == null) {
				// todo:
				throw new Exception("todo");
			}
			var folderId = file.FolderId;
			var folder = _storageRepository.Entities.UserFolders
				.SingleOrDefault(folderItem => folderItem.Id == folderId);
			if (folder == null) {
				// todo:
				throw new Exception("todo");
			}
			// todo:
			var index = folder.Name.LastIndexOf("\\", StringComparison.Ordinal);
			if (index >= 0) {
				folder.Name = folder.Name.Substring(index + 1);
			}

			if (folder.Name.Equals(GetUserRootFolderId(userId))) {
				return Path.Combine(folder.Name, file.Name);
			}

			while (true) {
				var currentFolder = _storageRepository.Entities.UserFolders
					.SingleOrDefault(folderItem => folderItem.Id == folderId);
				if (currentFolder == null) {
					// todo:
					throw new Exception("todo");
				}
				if (currentFolder.Id.Equals(GetUserRootFolderId(userId))) {
					return Path.Combine(currentFolder.Name, folder.Name, file.Name);
				}
				if (!folder.Name.Equals(currentFolder.Name)) {
					folder.Name = Path.Combine(currentFolder.Name, folder.Name);
				}
				folderId = currentFolder.ParentId;
			}
		}

		// todo: implement user space counter 
		private bool HasUserEnoughFreeSpace() {
			return true;
		}

		// todo: if there is no user folder on some server
		private string GetUserRootFolderName( string userId ) {
			return userId;
		}

		private string GetUserRootPath( LocalFileServer server, string userId ) {
			return Path.Combine(server.Path, GetUserRootFolderName(userId));
		}

		private string GetFolderServerPath(string userId, string folderId,
			LocalFileServer server) {
			return Path.Combine(server.Path, GetFolderPath(userId, folderId));
		}

		private string GetFolderPath(string userId, string folderId) {
			var folder = _storageRepository.Entities.UserFolders
				.SingleOrDefault(folderItem => folderItem.Id == folderId);
			if (folder == null) {
				// todo:
				throw new Exception("todo");
			}
			if (folder.Name.Equals(GetUserRootFolderId(userId))) {
				return folder.Name;
			}

			var folderName = folder.Name;

			while (true) {
				var currentFolder = _storageRepository.Entities.UserFolders
					.SingleOrDefault(folderItem => folderItem.Id == folderId);
				if (currentFolder == null) {
					// todo:
					throw new Exception("todo");
				}
				if (currentFolder.Id.Equals(GetUserRootFolderId(userId))) {
					return Path.Combine(currentFolder.Name, folderName);
				}
				if (!currentFolder.Name.Equals(folderName)) {
					folderName = Path.Combine(currentFolder.Name, folderName);
				}
				folderId = currentFolder.ParentId;
			}
		}

		private IEnumerable<LocalFileServer> GetFileServers() {
			return new LocalFileServerRepository().GetLocalFileServers();
		}

		#endregion Private methods
	}
}
