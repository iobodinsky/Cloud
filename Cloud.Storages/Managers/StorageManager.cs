using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Storages.Providers;

namespace Cloud.Storages.Managers {
	public class StorageManager {
		public IEnumerable<IStorage> GetStorages() {
			return new List<IStorage> {
				new DriveProvider(),
				new LocalLenevoProvider()
			};
			 
		} 
	}
}
