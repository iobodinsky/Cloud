using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.Managers;
using Cloud.Storages.Providers;

namespace Cloud.Storages.Repositories {
	public class StorageRepository : RepositoryBase, IFileRepository {
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

		public void Add( string userId, int cloudId, FullUserFile file ) {
			var cloud = ResolveStorageInstance(cloudId);
			cloud.AddFile(userId, file);
		}

		public IFile GetFileInfo( string userId, int cloudId, string fileId ) {
			throw new NotImplementedException();
		}

		public FullUserFile GetFile( string userId, int cloudId, string fileId ) {
			var cloud = ResolveStorageInstance(cloudId);
			return cloud.GetFile(userId, fileId);
		}

		public IFile Get( string userId, int cloudId, string fileId ) {
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
			var folders = new List<IFolder>();
			foreach (var storage in _storages) {
				folders.AddRange(storage.GetRootFolders(userId));
			}

			return folders;
		}

		public string GetRootFolderId( string userId ) {
			return userId;
		}

		public void UpdateName( string userId, int cloudId, string fileId, string newfileName ) {
			var fileToUpdate = Entities.UserFiles.SingleOrDefault(
				file => file.Id == fileId && file.UserId == userId);
			if (fileToUpdate == null) {
				// todo
				throw new Exception("todo");
			}

			// Rename file on servers
			var oldfileName = fileToUpdate.Name;
			var extention = Path.GetExtension(oldfileName);
			newfileName += extention;
			var serverManager = new LocalFileServerManager();
			serverManager.RenameFile(userId, fileId, oldfileName, newfileName);

			// Rename file in Db
			fileToUpdate.Name = newfileName;
			Entities.UserFiles.Attach(fileToUpdate);
			var entry = Entities.Entry(fileToUpdate);
			entry.Property(file => file.Name).IsModified = true;
			SaveChanges();
		}

		public void Delete( string userId, int cloudId, string fileId ) {
			var fileToDelete = Entities.UserFiles.SingleOrDefault(
				file => file.Id == fileId && file.UserId == userId);
			if (fileToDelete == null) {
				// todo:
				throw new Exception("todo");
			}

			// Delete file from all servers

			var serverManager = new LocalFileServerManager();
			serverManager.DeleteFile(userId, fileToDelete.Name);

			// Delete file from db
			Entities.UserFiles.Attach(fileToDelete);
			Entities.UserFiles.Remove(fileToDelete);
			SaveChanges();
		}

		#endregion IFileRepository implementation
	}
}