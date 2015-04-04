using System.Collections.Generic;
using System.IO;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Repositories;

namespace Cloud.Repositories.Common
{
    public class ServerManager
    {
        #region Fields

        private readonly FileServerRepository _fileServerRepository;

        #endregion Fields

        public ServerManager()
        {
            _fileServerRepository = new FileServerRepository();
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
                using (var createdFileStream = File.Open(filePath, FileMode.CreateNew))
                {
                    // Save the same file stream on all servers
                    fileStream.Position = 0;

                    fileStream.CopyTo(createdFileStream);
                }
            }

            return true;
        }

        public bool DeleteFile(string userId, string fileName)
        {
            foreach (var fileServer in GetFileServers())
            {
                var serverFilePath = GetUserFileServerPath(fileServer.Path, userId, fileName);
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
        
        private string GetUserFileServerPath(string serverPath, string userId, string fileName)
        {
            return Path.Combine(serverPath, GetUserPath(userId), fileName);
        }

        private IEnumerable<FileServer> GetFileServers()
        {
            return _fileServerRepository.GetFileServers();
        }

        #endregion Private methods
    }
}
