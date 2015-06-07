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
				AccessToken = "9kJKtEcgWjIAAAAAAAAAUsuhDgbx0T-V0eICdkd8cgi5gifdJYYql3zmlmYqvn-4",
				RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
			};

			var client = new Client(options);

			//// Get the OAuth Request Url
			//var authRequestUrl = await client.Core.OAuth2.AuthorizeAsync("code");
			//Process.Start(authRequestUrl.AbsoluteUri);

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
