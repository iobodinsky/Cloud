using System;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Storages.DataContext;

namespace Cloud.Storages.Repositories {
	public class DropboxUserTokenRepository : RepositoryBase {
		public async Task<DropboxUserToken> GetTokenAsync(string key) {
			var token = await Task.Run(() =>
			Entities.DropboxUserTokens
				.SingleOrDefault(tokenItem => tokenItem.UserId.Equals(key)));

			return token;
		}

		public async Task DeleteAsync(string key) {
			await Task.Run(() => {
				var tokenToDelete = Entities.DropboxUserTokens.
				SingleOrDefault(token => token.UserId.Equals(key));
				if (tokenToDelete == null) {
					// todo:
					throw new Exception("todo");
				}

				Entities.DropboxUserTokens.Attach(tokenToDelete);
				Entities.DropboxUserTokens.Remove(tokenToDelete);
				Entities.SaveChanges();
			});
		}
	}
}
