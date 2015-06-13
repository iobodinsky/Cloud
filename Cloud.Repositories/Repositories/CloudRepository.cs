using System.Collections.Generic;
using System.Linq;
using Cloud.Common.Interfaces;

namespace Cloud.Repositories.Repositories {
	public class CloudRepository : RepositoryBase {
		public IEnumerable<IStorage> GetCloudStorages() {
			var cloudServers = Entities.Storages.Where(cloud => cloud.IsActive);
			var ctorages = new List<IStorage>();
			foreach (var cloudServer in cloudServers) {
				ctorages.Add(ResolveStorageInstance(cloudServer.Id, cloudServer.ClassName));
			}

			return ctorages;
		}
	}
}