using System.Collections.Generic;
using System.Linq;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
{
    public class LocalFileServerRepository : RepositoryBase
    {
        public LocalFileServer GetLocalFileServer(int serverId)
        {
            return Entities.LocalFileServers.SingleOrDefault(
                server => server.ServerId == serverId);
        }

        public IEnumerable<LocalFileServer> GetLocalFileServers()
        {
            return Entities.LocalFileServers;
        }

        public bool AddLocalFileServer(LocalFileServer server)
        {
            return Add(server, true);
        }

        public bool UpdateLocalFileSrver(LocalFileServer serverToUpdate)
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