using System.Collections.Generic;
using System.Linq;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
{
    public class FileServerRepository : RepositoryBase
    {
        public FileServer GetFileServer(int serverId)
        {
            return Entities.FileServers.SingleOrDefault(
                server => server.ServerId == serverId);
        }

        public IEnumerable<FileServer> GetFileServers()
        {
            return Entities.FileServers;
        }

        public bool AddFileServer(FileServer server)
        {
            return Add(server, true);
        }

        public bool UpdateFileSrver(FileServer serverToUpdate)
        {
            Entities.FileServers.Attach(serverToUpdate);
            var entry = Entities.Entry(serverToUpdate);
            entry.Property(server => server.Name).IsModified = true;
            entry.Property(server => server.Path).IsModified = true;
            SaveChanges();

            return true;
        }
    }
}
