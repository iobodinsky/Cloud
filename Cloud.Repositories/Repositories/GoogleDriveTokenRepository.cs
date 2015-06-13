using System;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories {
	 public class GoogleDriveTokenRepository : RepositoryBase {
		 public async Task<GoogleDriveUserToken> GetTokenAsync(string key) {
			 var token = await Task.Run(() =>
			 Entities.GoogleDriveUserTokens
				 .SingleOrDefault(tokenItem => tokenItem.UserId.Equals(key)));

			 return token;
		 }

		 public async Task DeleteAsync(string key) {
			 await Task.Run(() => {
				 var tokenToDelete = Entities.GoogleDriveUserTokens.
					 SingleOrDefault(token => token.UserId.Equals(key));
				 if (tokenToDelete == null) {
					 // todo:
					 throw new Exception("todo");
				 }

				 Entities.GoogleDriveUserTokens.Attach(tokenToDelete);
				 Entities.GoogleDriveUserTokens.Remove(tokenToDelete);
				 Entities.SaveChanges();
			 });
		 }
	}
}
