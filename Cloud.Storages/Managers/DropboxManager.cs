using System.Configuration;
using System.Threading.Tasks;
using Cloud.Storages.Resources;
using DropboxRestAPI;

namespace Cloud.Storages.Managers {
	internal class DropboxManager {
		public async Task<Client> GetClient() {
			var options = new Options {
				ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
				ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
				AccessToken = "9kJKtEcgWjIAAAAAAAAAR6p1BUBGG2pLN5LPc79DHbZLiezIMTI5TPAHgqqS3SbE",
				RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
			};

			// Initialize a new Client (without an AccessToken)
			var client = new Client(options);

			//// Get the OAuth Request Url
			//var authRequestUrl = await client.Core.OAuth2.AuthorizeAsync("code");
			//Process.Start(authRequestUrl.AbsoluteUri);

			return client;
		}

	}
}
