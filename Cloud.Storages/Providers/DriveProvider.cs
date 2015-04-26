using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.Managers;
using Cloud.Storages.Models;
using Cloud.Storages.Resources;

namespace Cloud.Storages.Providers
{
    public class DriveProvider : IStorage
    {
        #region Private fields

        private readonly DriveManager _manager = new DriveManager();

        #endregion Private fields

        #region IStorage implementation

        public bool Add(string userId, FullUserFile file)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFile> GetRootFiles(string userId)
        {
            var service = _manager.BuildServiceAsync(userId);
            var request = service.Files.List();

            request.MaxResults = int.Parse(
                ConfigurationManager.AppSettings[DriveSearchFilters.FilesMaxResults]);

            request.Q = _manager.ConstructSearchQuery(
                DriveSearchFilters.NoTrash, DriveSearchFilters.SearchRoot, DriveSearchFilters.SearchNotFolders);

            return request.Execute().Items.Select(file => new DriveFile
            {
                Id = file.Id,
                Name = file.Title,
                LastModifiedDateTime = file.LastViewedByMeDate == null
                    ? new DateTime()
                    : file.LastViewedByMeDate.Value,
                AddedDateTime = file.CreatedDate == null
                    ? new DateTime()
                    : file.CreatedDate.Value,
                DownloadUrl = file.WebContentLink
            });
        }

        public IEnumerable<IFile> GetFilesIn(string userId, string folder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFolder> GetRootFolders(string userId)
        {
            var service = _manager.BuildServiceAsync(userId);
            var request = service.Files.List();

            request.MaxResults = int.Parse(ConfigurationManager.AppSettings[DriveSearchFilters.FilesMaxResults]);
            request.Q = _manager.ConstructSearchQuery(
                DriveSearchFilters.NoTrash, DriveSearchFilters.SearchRoot, DriveSearchFilters.SearchFolders);

            return request.Execute().Items.Select(file => new Folder
            {
                Id = file.Id,
                Name = file.Title
            });
        }

        public IEnumerable<IFolder> GetFoldersIn(string userId, string folder)
        {
            throw new NotImplementedException();
        }

        public IFile GetFileInfo(string userId, string fileId)
        {
            throw new NotImplementedException();
        }

        public FullUserFile GetFile(string userId, string fileId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateName(string userId, string fileId, string newfileName)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string userId, string fileId)
        {
            throw new NotImplementedException();
        }

        #endregion IStorage implementation
    }
}