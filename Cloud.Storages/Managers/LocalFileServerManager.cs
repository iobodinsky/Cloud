using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Repositories;

namespace Cloud.Storages.Managers
{
    public class LocalFileServerManager : RepositoryBase
    {
        #region Fields

        private readonly LocalFileServerRepository _fileServerRepository;

        #endregion Fields

        public LocalFileServerManager()
        {
            _fileServerRepository = new LocalFileServerRepository();
        }

        #region Public methods

        public void CreateUserDirectory( string userId)
        {
            foreach (var fileServer in GetFileServers())
            {
                var newUserDirectoryPath = Path.Combine(fileServer.Path, GetUserPath(userId));
                Directory.CreateDirectory(newUserDirectoryPath);
            }
        }

        // todo: user return value
        public bool SaveFile(Stream fileStream, string fileNameWithExtension, string userId)
        {
            if (!HasUserEnoughFreeSpace()) return false;

            foreach (var fileServer in GetFileServers())
            {
                var filePath = Path.Combine(fileServer.Path, GetUserPath(userId), fileNameWithExtension);
                if (File.Exists(filePath)) throw new Exception("File already exist");
                using (var createdFileStream = File.Open(filePath, FileMode.CreateNew))
                {
                    // Save the same file stream on all servers
                    fileStream.Position = 0;

                    fileStream.CopyTo(createdFileStream);
                }
            }

            return true;
        }

        // todo: check if needed FileInfo or just Stream
        public FullUserFile GetFile(string userId, string fileId)
        {
            FullUserFile file = null;
            foreach (var fileServer in GetFileServers())
            {
                var filePath = GetFilePathById(fileServer.Path, userId, fileId);
                if (string.IsNullOrEmpty(filePath)) continue;

                file = new FullUserFile
                {
                    Stream = new FileStream(filePath, FileMode.Open)
                };
            }

            return file;
        }

        public bool RenameFile(string userId, string fileId, string oldFileName, string newFileName)
        {
            foreach (var fileServer in GetFileServers())
            {
                var fileServerPath = GetFilePathByName(fileServer.Path, userId, oldFileName);
                var serverNewFilePath = GetFilePathByName(fileServer.Path, userId, newFileName);
                if (File.Exists(serverNewFilePath)) return false;

                File.Move(fileServerPath, serverNewFilePath);
            }

            return true;
        }

        public bool DeleteFile(string userId, string fileName)
        {
            foreach (var fileServer in GetFileServers())
            {
                var serverFilePath = GetFilePathByName(fileServer.Path, userId, fileName);
                File.Delete(serverFilePath);
            }

            return true;
        }

        #endregion Public methods

        #region Private methods

        // todo: implement user space counter 
        private bool HasUserEnoughFreeSpace()
        {
            return true;
        }

        // todo: if there is no user folder on some server
        private string GetUserPath(string userId)
        {
            return userId;
        }
        
        private string GetFilePathByName(string serverPath, string userId, string fileName)
        {
            return Path.Combine(serverPath, GetUserPath(userId), fileName);
        }

        private string GetFilePathById(string serverPath, string userId, string fileId)
        {
            var fileInfo = Entities.UserFiles.
                SingleOrDefault(file => file.UserId == userId && file.Id == fileId);
            if (fileInfo == null) return null;

            var fileName = fileInfo.Name;
            return GetFilePathByName(serverPath, userId, fileName);
        }

        private IEnumerable<LocalFileServer> GetFileServers()
        {
            return _fileServerRepository.GetLocalFileServers();
        }

        #endregion Private methods
    }
}
