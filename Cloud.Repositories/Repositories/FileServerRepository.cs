using System.Collections.Generic;
using System.Linq;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
{
    public class FileServerRepository : RepositoryBase
    {
        public LocalFileServer GetFileServer(int serverId)
        {
            return Entities.LocalFileServers.SingleOrDefault(
                server => server.ServerId == serverId);
        }

        public IEnumerable<LocalFileServer> GetFileServers()
        {
            return Entities.LocalFileServers;
        }

        public bool AddFileServer(LocalFileServer server)
        {
            return Add(server, true);
        }

        public bool UpdateFileSrver(LocalFileServer serverToUpdate)
        {
            Entities.LocalFileServers.Attach(serverToUpdate);
            var entry = Entities.Entry(serverToUpdate);
            entry.Property(server => server.Name).IsModified = true;
            entry.Property(server => server.Path).IsModified = true;
            SaveChanges();

            return true;
        }
    }
}