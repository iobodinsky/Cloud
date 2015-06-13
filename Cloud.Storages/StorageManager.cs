using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Repositories.Repositories;

namespace Cloud.Storages {
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