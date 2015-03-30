using System.Collections.Generic;
using System.IO;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Models;

namespace Cloud.Repositories.Common
{
    internal class ServerManager
    {
        private readonly IEnumerable<FileServer> _servers;
        private readonly string _userId;
        private readonly double _userFreeSpace;

        public ServerManager(IEnumerable<FileServer> servers, string userId)
        {
            _servers = servers;
            _userId = userId;
        }

        // todo: user return value
        public SaveUserFileStatus SaveFile(Stream fileStream, string fileNameWithExtension)
        {
            if (!HasUserEnoughFreeSpace()) return SaveUserFileStatus.NoFreeUserSpace;

            foreach (var fileServer in _servers)
            {
                var filePath = Path.Combine(fileServer.Path, GetUserPath(), fileNameWithExtension);
                using (var file = File.Open(filePath, FileMode.CreateNew))
                    fileStream.CopyTo(file);   
            }

            return SaveUserFileStatus.Success;
        }

        // todo: implement user space counter 
        private bool HasUserEnoughFreeSpace()
        {
            return true;
        }

        private string GetUserPath()
        {
            return _userId;
        }
    }
}
