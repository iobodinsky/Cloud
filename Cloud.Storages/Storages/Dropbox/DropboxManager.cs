using System;
using System.Configuration;
using System.Threading.Tasks;
using Cloud.Storages.Repositories;
using Cloud.Storages.Resources;
using DropboxRestAPI;

namespace Cloud.Storages.Storages.Dropbox {
	internal class DropboxManager {
		public async Task<Client> GetClient( string userId ) {
			var options = new Options {
				ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
				ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
				RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
			};
			var client = new Client(options);

			var userToken = await new DropboxUserTokenRepository()
				.GetTokenAsync(userId);
			if (userToken == null) {
				var authRequestUrl = await client.Core.OAuth2
					.AuthorizeAsync(DropboxKeys.AuthorizeResponceType);
				// todo
				throw new Exception("Dropbox account unauthorised", 
					new Exception(authRequestUrl.AbsoluteUri));
			}

			client.UserAccessToken = userToken.AccessToken;
			
			return client;
		}

		public string ConstructEntityId(string path) {
			return path.Replace('/', '|');
		}

		public string ConstructEntityPath(string path) {
			return path.Replace('|', '/');
		}

		public string MakeValidPath( string path ) {
			return path.Replace("\\", "/")
				.Replace("//", "/");
		}
	}
}
