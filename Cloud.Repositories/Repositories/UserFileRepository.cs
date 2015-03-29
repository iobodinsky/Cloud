using System.Linq;
using Cloud.Repositories.Common;
using Cloud.Repositories.Models;

namespace Cloud.Repositories.Repositories
{
    public class UserFileRepository : RepositoryBase
    {
        private readonly FileServerRepository _fileServerRepository;
        private readonly UserFileInfoRepository _fileInfoRepository;

        public UserFileRepository()
        {
            _fileServerRepository = new FileServerRepository();
            _fileInfoRepository = new UserFileInfoRepository();
        }

        public bool AddUserFile(UserFile file)
        {
            // Save file on all physical servers
            var servers = _fileServerRepository.GetFileServers();
            if (servers == null || !servers.Any()) return false;

            foreach (var fileServer in servers)
            {
                var serverManager = new ServerManager(fileServer, file.UserFileInfo.UserId);
                serverManager.SaveFile(file.Stream, file.UserFileInfo.Name + file.FileType.Extension);
            }

            // Save file info to Db
            _fileInfoRepository.Add(file.UserFileInfo, true);

            return true;
        }
    }
}
