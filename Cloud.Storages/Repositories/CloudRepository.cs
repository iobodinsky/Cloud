using System.Collections.Generic;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Storages.DataContext;

namespace Cloud.Storages.Repositories {
	public class CloudRepository : RepositoryBase {
		public IEnumerable<CloudServer> GetCloudServers() {
			return Entities.CloudServers.Where(cloud => cloud.IsActive);
		}

		public IEnumerable<IStorage> GetCloudStorages() {
			var cloudServers = GetCloudServers();
			var clouds = new List<IStorage>();
			foreach (var cloudServer in cloudServers) {
				clouds.Add(ResolveStorageInstance(cloudServer.Id, cloudServer.ClassName));
			}

			return clouds;
		}
	}
}
