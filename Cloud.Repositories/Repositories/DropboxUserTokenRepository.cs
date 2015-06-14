using System;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories {
	public class DropboxUserTokenRepository : RepositoryBase {
		public async Task<DropboxUserToken> GetTokenAsync( string key ) {
			var token = await Task.Run(() =>
				Entities.DropboxUserTokens
					.SingleOrDefault(tokenItem => tokenItem.UserId.Equals(key)));

			return token;
		}

		public async Task DeleteAsync( string key ) {
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

		public async Task AddOrUpdateAsunc( DropboxUserToken dropboxToken, string key ) {
			var token = Entities.DropboxUserTokens.SingleOrDefault(
				tokenItem => tokenItem.UserId == key);
			if (token != null) {
				Entities.DropboxUserTokens.Attach(dropboxToken);
				var entry = Entities.Entry(dropboxToken);
				entry.Property(tokenItem => tokenItem.AccessToken).IsModified = true;
				entry.Property(tokenItem => tokenItem.TeamId).IsModified = true;
				entry.Property(tokenItem => tokenItem.TokenType).IsModified = true;
				entry.Property(tokenItem => tokenItem.Uid).IsModified = true;
				SaveChanges();
			} else {
				await AddAsync(dropboxToken, true);
			}
		}
	}
}