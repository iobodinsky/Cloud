using System;
using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;

namespace Cloud.Storages.Providers {
	internal class LocalLenevoProvider : IStorage {
		#region Private fields

		private readonly LocalFileServerManager _localFileServerManager;

		#endregion Private fields

		public LocalLenevoProvider() {
			_localFileServerManager = new LocalFileServerManager();
		}

		#region IStorage implementation

		public void AddFile( string userId, FullUserFile file ) {
			_localFileServerManager.AddFile(userId, file);
		}

		public void AddFolder( string userId, IFolder folder ) {
			_localFileServerManager.AddFolder(userId, folder, true);
		}

		public IEnumerable<IFile> GetRootFiles( string userId ) {
			return _localFileServerManager.GetRootFiles(userId);
		}

		public IEnumerable<IFolder> GetRootFolders( string userId ) {
			return _localFileServerManager.GetRootFolders(userId);
		}

		public IEnumerable<IFile> GetFilesIn( string userId, string folder ) {
			throw new NotImplementedException();
		}

		public IEnumerable<IFolder> GetFoldersIn( string userId, string folder ) {
			throw new NotImplementedException();
		}

		public IFile GetFileInfo( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public FullUserFile GetFile( string userId, string fileId ) {
			return _localFileServerManager.GetFullFile(userId, fileId);
		}

		public void UpdateName( string userId, string fileId, string newfileName ) {
			throw new NotImplementedException();
		}

		public void Delete( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		#endregion IStorage implementation
	}
}
