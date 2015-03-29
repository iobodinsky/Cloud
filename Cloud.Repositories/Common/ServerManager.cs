using System.IO;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Common
{
    internal class ServerManager
    {
        private readonly FileServer _fileServer;
        private readonly string _userId;
        private readonly double _userFreeSpace;

        public ServerManager(FileServer fileServer, string userId)
        {
            _fileServer = fileServer;
            _userId = userId;
        }

        public bool SaveFile(Stream fileStream, string fileNameWithExtension)
        {
            if (!HasUserEnoughFreeSpace()) return false;

            var filePath = Path.Combine(_fileServer.Path, GetUserPath(), fileNameWithExtension);
            using (var file = File.Open(filePath, FileMode.CreateNew))
                fileStream.CopyTo(file);

            return true;
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
