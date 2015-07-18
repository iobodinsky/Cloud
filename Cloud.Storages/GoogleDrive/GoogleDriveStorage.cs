using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Repositories.Repositories;
using Cloud.Storages.Resources;
using Google;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v2.Data;

namespace Cloud.Storages.GoogleDrive
{
    public class GoogleDriveStorage : IStorage
    {
        #region Private fields

        private readonly int _id;
        private readonly string _alias;
        private readonly GoogleDriveManager _manager;
        private readonly UserStorageRepository _userStoragesRepository;
        private readonly GoogleDriveTokenRepository _tokenRepository;

        #endregion Private fields

        public GoogleDriveStorage(int id, string alias)
        {
            _id = id;
            _alias = alias;
            _manager = new GoogleDriveManager();
            _userStoragesRepository = new UserStorageRepository();
            _tokenRepository = new GoogleDriveTokenRepository();
        }

        #region IStorage implementation

        public async Task AuthorizeAsync(string userId, string code)
        {
            await _manager.BuildServiceAsync(userId);
            await _userStoragesRepository.AddAsync(userId, _id);
        }

        public async Task DisconnectAsync(string userId)
        {
            await _userStoragesRepository.DeleteAsync(userId, _id);
            await _tokenRepository.DeleteAsync(userId);
        }

        public async Task<IFile> AddFileAsync(string userId, FullUserFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<IFolder> AddFolderAsync(string userId, IFolder folder)
        {
            var service = await _manager.BuildServiceAsync(userId);
            var newFolder = new File
            {
                Title = folder.Name,
                MimeType = GoogleDriveMimeTypes.Folder
            };

            if (folder.ParentId != null)
            {
                newFolder.Parents = new List<ParentReference>
                {
                    new ParentReference
                    {
                        Id = folder.ParentId
                    }
                };
            }

            var createdGoogleFile = await service.Files.Insert(newFolder).ExecuteAsync();
            var ceatedFolder = new UserFolder
            {
                Id = createdGoogleFile.Id,
                Name = createdGoogleFile.Title,
                ParentId = folder.ParentId,
                Storage = _alias
            };

            return ceatedFolder;
        }

        public async Task<FolderData> GetRootFolderDataAsync(string userId)
        {
            try
            {
                var service = await _manager.BuildServiceAsync(userId);
                var request = service.Files.List();

                request.MaxResults = int.Parse(
                    ConfigurationManager.AppSettings[DriveSearchFilters.FilesMaxResults]);
                request.Q = _manager.BuildSearchQuery(
                    DriveSearchFilters.NoTrash, DriveSearchFilters.SearchRoot);

                var foldersFiles = request.Execute().Items;
                var files = foldersFiles.Where(
                    folderFile => !folderFile.MimeType.Equals(DriveSearchFilters.FolderMimiType))
                    .Select(file => new UserFile
                    {
                        Id = file.Id,
                        Name = file.Title,
                        LastModifiedDateTime = file.LastViewedByMeDate == null
                            ? new DateTime()
                            : file.LastViewedByMeDate.Value,
                        AddedDateTime = file.CreatedDate == null
                            ? new DateTime()
                            : file.CreatedDate.Value,
                        DownloadUrl = file.WebContentLink,
                        Storage = _alias
                    });
                var folders = foldersFiles.Where(
                    folderFile => folderFile.MimeType.Equals(DriveSearchFilters.FolderMimiType))
                    .Select(folder => new UserFolder
                    {
                        Id = folder.Id,
                        Name = folder.Title,
                        Storage = _alias
                    });
                var currentFolder = new UserFolder
                {
                    Id = "root",
                    Storage = _alias
                };

                return new FolderData
                {
                    Files = files,
                    Folders = folders,
                    Folder = currentFolder,
                    Storage = _alias
                };
            }
            catch (TokenResponseException)
            {
                throw new Exception("Drive unauthorized");
            }
            catch (GoogleApiException)
            {
                throw new Exception("Drive unauthorized");
            }
        }

        public async Task<FolderData> GetFolderDataAsync(string userId, string folderId)
        {
            try
            {
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
                    .Select(file => new UserFile
                    {
                        Id = file.Id,
                        Name = file.Title,
                        LastModifiedDateTime = file.LastViewedByMeDate == null
                            ? new DateTime()
                            : file.LastViewedByMeDate.Value,
                        AddedDateTime = file.CreatedDate == null
                            ? new DateTime()
                            : file.CreatedDate.Value,
                        DownloadUrl = file.WebContentLink,
                        Storage = _alias
                    });
                var folders = foldersFiles.Where(
                    folderFile => folderFile.MimeType.Equals(DriveSearchFilters.FolderMimiType))
                    .Select(folder => new UserFolder
                    {
                        Id = folder.Id,
                        Name = folder.Title,
                        Storage = _alias
                    });
                var currentFolder = new UserFolder
                {
                    Id = folderId,
                    Storage = _alias
                };

                return new FolderData
                {
                    Files = files,
                    Folders = folders,
                    Folder = currentFolder,
                    Storage = _alias
                };
            }
            catch (TokenResponseException)
            {
                throw new Exception("Drive unauthorized");
            }
            catch (GoogleApiException)
            {
                throw new Exception("Drive unauthorized");
            }
        }

        public async Task<IFile> GetFileInfoAsync(string userId, string fileId)
        {
            throw new NotImplementedException();
        }

        public async Task<FullUserFile> GetFileAsync(string userId, string fileId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateFileNameAsync(string userId, string fileId, string newfileName)
        {
            try
            {
                var service = await _manager.BuildServiceAsync(userId);
                var file = new File
                {
                    Title = newfileName
                };
                var request = service.Files.Patch(file, fileId);
                var responce = request.Execute();
                return responce.Title;
            }
            catch (TokenResponseException)
            {
                throw new Exception("Drive unauthorized");
            }
            catch (GoogleApiException)
            {
                throw new Exception("Drive unauthorized");
            }
        }

        public async Task<string> UpdateFolderNameAsync(string userId, string folderId, string newFolderName)
        {
            try
            {
                return await UpdateFileNameAsync(userId, folderId, newFolderName);
            }
            catch (TokenResponseException)
            {
                throw new Exception("Drive unauthorized");
            }
            catch (GoogleApiException)
            {
                throw new Exception("Drive unauthorized");
            }
        }

        public async Task DeleteFileAsync(string userId, string fileId)
        {
            try
            {
                var service = await _manager.BuildServiceAsync(userId);
                service.Files.Delete(fileId).Execute();
            }
            catch (TokenResponseException)
            {
                throw new Exception("Drive unauthorized");
            }
            catch (GoogleApiException)
            {
                throw new Exception("Drive unauthorized");
            }
        }

        public async Task DeleteFolderAsync(string userId, string folderId)
        {
            try
            {
                await DeleteFileAsync(userId, folderId);
            }
            catch (TokenResponseException)
            {
                throw new Exception("Drive unauthorized");
            }
            catch (GoogleApiException)
            {
                throw new Exception("Drive unauthorized");
            }
        }

        #endregion IStorage implementation
    }
}