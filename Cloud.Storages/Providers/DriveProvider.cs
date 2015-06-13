using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;
using Cloud.Storages.Resources;
using Google.Apis.Drive.v2.Data;

namespace Cloud.Storages.Providers {
	internal class DriveProvider : IStorage {
		#region Private fields

		private readonly DriveManager _manager;

		#endregion Private fields

		public DriveProvider() {
			_manager = new DriveManager();
		}

		#region IStorage implementation

		public void Authorize() {
			throw new NotImplementedException();
		}

		public async Task<IFile> AddFileAsync( string userId, FullUserFile file ) {

			// todo: Adding files only to cloud
			//var body = new Google.Apis.Drive.v2.Data.File {
			//	Title = file.UserFile.Name,
			//};

			//var service = _manager.BuildServiceAsync(userId);
			//var request = service.Files.Insert(body, file.Stream, string.Empty);
			//request.Upload();

			throw new NotImplementedException();
		}

		public async Task<IFolder> AddFolderAsync( string userId, IFolder file ) {
			throw new NotImplementedException();
		}

		public async Task<FolderData> GetRootFolderDataAsync( string userId ) {
			var service = await _manager.BuildServiceAsync(userId);
			var request = service.Files.List();

			request.MaxResults = int.Parse(
				ConfigurationManager.AppSettings[DriveSearchFilters.FilesMaxResults]);
			request.Q = _manager.BuildSearchQuery(
				DriveSearchFilters.NoTrash, DriveSearchFilters.SearchRoot);

			var foldersFiles = request.Execute().Items;
			var files = foldersFiles.Where(
				folderFile => !folderFile.MimeType.Equals(DriveSearchFilters.FolderMimiType))
				.Select(file => new UserFile {
					Id = file.Id,
					Name = file.Title,
					LastModifiedDateTime = file.LastViewedByMeDate == null
						? new DateTime()
						: file.LastViewedByMeDate.Value,
					AddedDateTime = file.CreatedDate == null
						? new DateTime()
						: file.CreatedDate.Value,
					DownloadUrl = file.WebContentLink,
					CloudId = 1
				});
			var folders = foldersFiles.Where(
				folderFile => folderFile.MimeType.Equals(DriveSearchFilters.FolderMimiType))
				.Select(folder => new UserFolder {
					Id = folder.Id,
					Name = folder.Title,
					CloudId = 1
				});
			var currentFolder = new UserFolder {
				Id = "root",
				CloudId = 1
			};

			return new FolderData {
				Files = files,
				Folders = folders,
				Folder = currentFolder,
				CloudId = 1
			};
		}

		public async Task<FolderData> GetFolderDataAsync( string userId, string folderId ) {
			var service = await _manager.BuildServiceAsync(userId);
			var request = service.Files.List();

			request.MaxResults = int.Parse(
				ConfigurationManager.AppSettings[DriveSearchFilters.FilesMaxResults]);
			request.Q = _manager.BuildSearchQuery(
				DriveSearchFilters.NoTrash,
				_manager.ConstructInParentsQuery(folderId));

			var foldersFiles = request.Execute().Items;
			var files = foldersFiles.Where(
				folderFile => !folderFile.MimeType.Equals(DriveSearchFilters.FolderMimiType))
				.Select(file => new UserFile {
					Id = file.Id,
					Name = file.Title,
					LastModifiedDateTime = file.LastViewedByMeDate == null
						? new DateTime()
						: file.LastViewedByMeDate.Value,
					AddedDateTime = file.CreatedDate == null
						? new DateTime()
						: file.CreatedDate.Value,
					DownloadUrl = file.WebContentLink,
					CloudId = 1
				});
			var folders = foldersFiles.Where(
				folderFile => folderFile.MimeType.Equals(DriveSearchFilters.FolderMimiType))
				.Select(folder => new UserFolder {
					Id = folder.Id,
					Name = folder.Title,
					CloudId = 1
				});
			var currentFolder = new UserFolder {
				Id = folderId,
				CloudId = 1
			};

			return new FolderData {
				Files = files,
				Folders = folders,
				Folder = currentFolder
			};
		}

		public async Task<IFile> GetFileInfoAsync( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public async Task<FullUserFile> GetFileAsync( string userId, string fileId ) {
			throw new NotImplementedException();
		}

		public async Task<string> UpdateFileNameAsync( string userId, string fileId, string newfileName ) {
			var service = await _manager.BuildServiceAsync(userId);
			var file = new File {
				Title = newfileName
			};
			var request = service.Files.Patch(file, fileId);
			var responce = request.Execute();

			return responce.Title;
		}

		public async Task<string> UpdateFolderNameAsync( string userId, string folderId, string newFolderName ) {
			return await UpdateFileNameAsync(userId, folderId, newFolderName);
		}

		public async Task<bool> DeleteFileAsync( string userId, string fileId ) {
			var service = await _manager.BuildServiceAsync(userId);
			service.Files.Delete(fileId).Execute();

			return true;
		}

		public async Task<bool> DeleteFolderAsync( string userId, string folderId ) {
			return await DeleteFileAsync(userId, folderId);
		}

		#endregion IStorage implementation
	}
}