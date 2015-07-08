using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.Resources;

namespace Cloud.Storages.Dropbox
{
    public class DropboxStorage : IStorage
    {
        private readonly int _id;

        private readonly DropboxManager _manager;

        public DropboxStorage(int id)
        {
            _id = id;
            _manager = new DropboxManager(id);
        }

        public async Task<string> GetDownloadUrl(string userId, string fileId)
        {
            var client = await _manager.GetClient(userId);
            return (await client.Core.Metadata.MediaAsync(
                _manager.ConstructEntityPath(fileId))).url;
        }

        #region IStorage implementation

        public async Task AuthorizeAsync(string userId, string code)
        {
            await _manager.AuthorizeAsync(userId, code);
        }

        public async Task DisconnectAsync(string userId)
        {
            await _manager.DisconnectAsync(userId, _id);
        }

        public async Task<IFile> AddFileAsync(string userId, FullUserFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<IFolder> AddFolderAsync(string userId, IFolder folder)
        {
            throw new NotImplementedException();
        }

        public async Task<FolderData> GetRootFolderDataAsync(string userId)
        {
            var client = await _manager.GetClient(userId);
            var rootFilesFolders = await client.Core.Metadata.MetadataAsync(
                DropboxKeys.RootFolderPath);
            var folders = new List<IFolder>();
            var files = new List<IFile>();
            var folderData = new FolderData {StorageId = _id};
            foreach (var folderFile in rootFilesFolders.contents)
            {
                if (folderFile.is_dir)
                {
                    folders.Add(new UserFolder
                    {
                        StorageId = _id,
                        Id = _manager.ConstructEntityId(folderFile.path),
                        UserId = userId,
                        Name = folderFile.Name
                    });
                }
                else
                {
                    files.Add(new UserFile
                    {
                        StorageId = _id,
                        Name = folderFile.Name,
                        Id = _manager.ConstructEntityId(folderFile.path),
                        UserId = userId
                    });
                }
            }
            var folder = new UserFolder
            {
                Id = _manager.ConstructEntityId(DropboxKeys.RootFolderPath),
                StorageId = _id
            };
            folderData.Folders = folders;
            folderData.Files = files;
            folderData.Folder = folder;

            return folderData;
        }

        public async Task<FolderData> GetFolderDataAsync(string userId, string folderId)
        {
            var client = await _manager.GetClient(userId);
            var filesFolders = await client.Core.Metadata.MetadataAsync(
                _manager.ConstructEntityPath(folderId));
            var folders = new List<IFolder>();
            var files = new List<IFile>();
            var folderData = new FolderData {StorageId = _id};
            foreach (var folderFile in filesFolders.contents)
            {
                if (folderFile.is_dir)
                {
                    folders.Add(new UserFolder
                    {
                        StorageId = _id,
                        Id = _manager.ConstructEntityId(folderFile.path),
                        UserId = userId,
                        Name = folderFile.Name,
                    });
                }
                else
                {
                    files.Add(new UserFile
                    {
                        StorageId = _id,
                        Name = folderFile.Name,
                        Id = _manager.ConstructEntityId(folderFile.path),
                        UserId = userId,
                    });
                }
            }
            var folder = new UserFolder
            {
                Id = _manager.ConstructEntityId(folderId),
                StorageId = _id
            };
            folderData.Folders = folders;
            folderData.Files = files;
            folderData.Folder = folder;

            return folderData;
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
            var client = await _manager.GetClient(userId);
            var oldfilePathWithName = _manager.ConstructEntityPath(fileId);
            var fileExtention = Path.GetExtension(oldfilePathWithName);
            var oldfilePath = Path.GetDirectoryName(oldfilePathWithName);
            if (string.IsNullOrEmpty(oldfilePath))
            {
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
            return _manager.ConstructEntityId(responce.path);
        }

        public async Task<string> UpdateFolderNameAsync(string userId, string folderId, string newFolderName)
        {
            var client = await _manager.GetClient(userId);
            var oldFolderPathWithName = _manager.ConstructEntityPath(folderId);
            var oldfilePath = Path.GetDirectoryName(oldFolderPathWithName);
            if (string.IsNullOrEmpty(oldfilePath))
            {
                // todo:
                throw new Exception("todo");
            }

            var newFilePathWithName = _manager.MakeValidPath(
                Path.Combine(oldfilePath, newFolderName));
            var responce = await client.Core.FileOperations.MoveAsync(oldFolderPathWithName, newFilePathWithName);

            // todo: validation
            //if (responce) {

            //}

            return _manager.ConstructEntityId(responce.path);
        }

        public async Task DeleteFileAsync(string userId, string fileId)
        {
            var client = await _manager.GetClient(userId);
            var response = await client.Core.FileOperations.DeleteAsync(
                _manager.ConstructEntityPath(fileId));
            if (response.is_deleted == false)
            {
                // todo:
                throw new Exception("todo");
            }
        }

        public async Task DeleteFolderAsync(string userId, string folderId)
        {
            var client = await _manager.GetClient(userId);
            var response = await client.Core.FileOperations.DeleteAsync(
                _manager.ConstructEntityPath(folderId));
            if (response.is_deleted == false)
            {
                // todo:
                throw new Exception("todo");
            }
        }

        #endregion IStorage implementation
    }
}