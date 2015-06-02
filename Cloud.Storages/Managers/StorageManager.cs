using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Storages.Repositories;

namespace Cloud.Storages.Managers {
	public class StorageManager {
		private readonly CloudRepository _cloudRepository;

		public StorageManager() {
			_cloudRepository = new CloudRepository();
		}

		public IEnumerable<IStorage> GetStorages() {
			return _cloudRepository.GetCloudStorages();
		}
	}
}
