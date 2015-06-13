using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cloud.Storages.Resources;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Cloud.Storages.Storages.GoogleDrive {
	internal class DriveManager {
		#region Public methods

		/// <summary>
		/// Returns Drive service
		/// </summary>
		public async Task<DriveService> BuildServiceAsync( string userId ) {
			var initializer = await GetInitializerFor(userId);
			return new DriveService(initializer);
		}

		/// <summary>
		/// Constructs quesry for searching spesific files
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public string BuildSearchQuery( params string[] parameters ) {
			if (parameters == null || !parameters.Any()) return string.Empty;

			var query = new StringBuilder();
			foreach (var parameter in parameters) {
				query.Append(" and ");
				query.Append(parameter);
			}

			//Remove first word ' and '
			query.Remove(0, 5);

			return query.ToString();
		}

		public string ConstructInParentsQuery( string folderId ) {
			return string.Format("'{0}' {1}", folderId, DriveSearchFilters.InParents);
		}

		#endregion Public methods

		#region Private methods

		/// <summary> 
		/// Returns the request initializer required for authorized requests. 
		/// </summary>
		private async Task<BaseClientService.Initializer> GetInitializerFor( string userId ) {
			try {
				var secrets = new ClientSecrets {
					ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DriveClientId],
					ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DriveClientSecret]
				};

				var credentialPersistanceStore = GetPersistentCredentialStore();

				var userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					secrets, new[] {DriveService.Scope.Drive}, userId,
					CancellationToken.None, credentialPersistanceStore);

				var initializer = new BaseClientService.Initializer {
					HttpClientInitializer = userCredential,
					ApplicationName = ConfigurationManager.AppSettings[AppSettingKeys.DriveApplicationUserAgent]
				};

				return initializer;
			} catch (TokenResponseException) {
				throw new Exception("todo: Google rejected");
			} catch (Exception ex) {
				return null;
			}
		}

		// todo: get server folder path from Db
		/// <summary> 
		/// Returns a persistent data store for user's credentials.
		/// </summary>
		private IDataStore GetPersistentCredentialStore() {
			return new DbDataStore();
		}

		#endregion Private methods
	}
}