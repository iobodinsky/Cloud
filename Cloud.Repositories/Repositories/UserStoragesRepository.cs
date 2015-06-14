using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories {
	public class UserStoragesRepository : RepositoryBase {
		public IEnumerable<Storage> GetAvailableUserStorages( string userId ) {
			return Entities.AspNetUsers_Storages.Where(
				userStorage => userStorage.UserId == userId)
				.Select(userStorage => userStorage.Storage);
		}

		public async Task AddAsync( string userId, int storageId ) {
			await Task.Run(() => {
				Entities.AspNetUsers_Storages.Add(new AspNetUsers_Storages {
					UserId = userId,
					StorageId = storageId
				});

				Entities.SaveChanges();
			});
		}
	}
}