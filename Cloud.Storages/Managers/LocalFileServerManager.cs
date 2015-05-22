using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Managers;
using Cloud.Common.Models;
using Cloud.Common.Resources;
using Cloud.Storages.DataContext;
using Cloud.Storages.Repositories;

namespace Cloud.Storages.Managers {
	public class LocalFileServerManager {
		#region Fields

		private readonly LocalFileServerRepository _fileServerRepository;

		#endregion Fields

		public LocalFileServerManager() {
			_fileServerRepository = new LocalFileServerRepository();
		}

		#region Public methods

		public void AddFile(string userId, FullUserFile file) {
			// Save file on all physical servers
			SaveFile(file.Stream, file.UserFile.Name, file.UserFile.UserId);

			// todo: try catch for undo if db failed
			// Save file info to Db
			_fileServerRepository.Add(file.UserFile as UserFile, true);
		}

		public void AddFolder( string userId, IFolder entity, bool isAutoSave ) {
			// Save folder on all physical servers
			var folderId = new IdGenerator().ForFolder();
			entity.Id = folderId;
			CreateFolder(userId, entity);

			// todo: try catch for undo if db failed
			// todo: entity as UserFolder
			// Save file info to Db
			_fileServerRepository.Add(entity as UserFolder, isAutoSave);
		}

		public IEnumerable<IFile> GetRootFiles( string userId ) {
			var rootFolderId = GetUserRootFolderId(userId);
			return _fileServerRepository.Entities.UserFiles
				.Where(file => file.UserId == userId && file.FolderId == rootFolderId);
		}

		public IEnumerable<IFolder> GetRootFolders( string userId ) {
			var rootFolderId = GetUserRootFolderId(userId);
			return _fileServerRepository.Entities.UserFolders
				.Where(folder => folder.UserId == userId &&
				                 folder.ParentId == rootFolderId);
		}

		public void CreateUserRootDirectory( string userId ) {
			foreach (var fileServer in GetFileServers()) {
				Directory.CreateDirectory(GetUserRootPath(fileServer, userId));
			}

			// Save user root folder to Db
			var userRootFolder = new UserFolder {
				Id = userId,
				Name = userId,
				ParentId = string.Empty,
				UserId = userId
			};
			_fileServerRepository.Add(userRootFolder, true);
		}

		// todo: check if needed FileInfo or just Stream
		public FullUserFile GetFile( string userId, string fileId ) {
			FullUserFile file = null;
			foreach (var fileServer in GetFileServers()) {
				var filePath = GetFilePathById(fileServer.Path, userId, fileId);
				if (string.IsNullOrEmpty(filePath)) continue;

				file = new FullUserFile {
					Stream = new FileStream(filePath, FileMode.Open)
				};
			}

			return file;
		}

		public void RenameFile( string userId, string fileId, string oldFileName, string newFileName ) {
			//foreach (var fileServer in GetFileServers()) {
			//	var fileServerPath = Get(fileServer.Path, userId, oldFileName);
			//	var serverNewFilePath = GetFilePathByName(fileServer.Path, userId, newFileName);
			//	if (File.Exists(serverNewFilePath)) {
			//		// todo:
			//		throw new Exception("todo");
			//	}

			//	File.Move(fileServerPath, serverNewFilePath);
			//}
		}

		public bool DeleteFile( string userId, string fileName ) {
			//foreach (var fileServer in GetFileServers()) {
			//	var serverFilePath = GetFilePathByName(fileServer.Path, userId, fileName);
			//	File.Delete(serverFilePath);
			//}

			return true;
		}

		#endregion Public methods

		#region Private methods

		private void CreateFolder(string userId, IFolder folder) {
			var servers = GetFileServers();
			if (!servers.Any()) {
				// todo:
				throw new Exception("todo");
			}
			var folderPath = GetUserFolderPath(userId, folder.Name, folder.ParentId);
			if (Directory.Exists(Path.Combine(servers.First().Path, folderPath))) {
				// todo:
				throw new Exception("todo");
			}
			foreach (var server in servers) {
				Directory.CreateDirectory(Path.Combine(server.Path, folderPath));
			}
		}

		private string GetUserFolderPath(string userId, string folderName, string parentFolderId) {
			while (true) {
				var currentFolder = _fileServerRepository.Entities.UserFolders
					.SingleOrDefault(folder => folder.Id == parentFolderId);
				if (currentFolder == null) {
					// todo:
					throw new Exception("todo");
				}
				if (currentFolder.Id.Equals(GetUserRootFolderId(userId))) {
					return Path.Combine(currentFolder.Name, folderName);
				}
				folderName = Path.Combine(currentFolder.Name, folderName);
				parentFolderId = currentFolder.ParentId;
			}
		}

		// todo: user return value
		private void SaveFile(Stream fileStream, string fileNameWithExtension, string userId) {
			if (!HasUserEnoughFreeSpace()) {
				// todo:
				throw new Exception("todo");
			}

			foreach (var fileServer in GetFileServers()) {
				var filePath = Path.Combine(GetUserRootPath(fileServer, userId), fileNameWithExtension);
				if (File.Exists(filePath)) throw new Exception("File already exist");
				using (var createdFileStream = File.Open(filePath, FileMode.CreateNew)) {
					// Save the same file stream on all servers
					fileStream.Position = 0;

					fileStream.CopyTo(createdFileStream);
				}
			}
		}

		// todo: implement user space counter 
		private bool HasUserEnoughFreeSpace() {
			return true;
		}

		// todo: if there is no user folder on some server
		private string GetUserFolder( string userId ) {
			return userId;
		}

		private string GetUserRootPath(LocalFileServer server, string userId) {
			return Path.Combine(server.Path, GetUserFolder(userId));
		}

		private string GetFilePathById( string serverPath, string userId, string fileId ) {
			//var fileInfo = _fileServerRepository.Entities.UserFiles.
			//	SingleOrDefault(file => file.UserId == userId && file.Id == fileId);
			//if (fileInfo == null) return null;

			//var fileName = fileInfo.Name;
			//return GetFilePathByName(serverPath, userId, fileName);
			return string.Empty;
		}

		private IEnumerable<LocalFileServer> GetFileServers() {
			return _fileServerRepository.GetLocalFileServers();
		}

		private string GetUserRootFolderId( string userId ) {
			var userRootFolder = _fileServerRepository.Entities.UserFolders
				.SingleOrDefault(folder => folder.Id == userId);
			if (userRootFolder == null) throw new Exception(ErrorMessages.InvalidUserRootFolder);
			return userRootFolder.Id;
		}

		#endregion Private methods
	}
}
