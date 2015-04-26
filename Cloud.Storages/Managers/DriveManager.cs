using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cloud.Storages.Resources;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Cloud.Storages.Managers
{
    internal class DriveManager
    {
        #region Public methods

        /// <summary>
        /// Returns Drive service
        /// </summary>
        public DriveService BuildServiceAsync(string userId)
        {
            var initializer = GetInitializerFor(userId).Result;
            return new DriveService(initializer);
        }

        /// <summary>
        /// Constructs quesry for searching spesific files
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ConstructSearchQuery(params string[] parameters)
        {
            if (parameters == null || !parameters.Any()) return string.Empty;

            var query = new StringBuilder();
            foreach (var parameter in parameters)
            {
                query.Append(" and");
                query.Append(parameter);
            }

            //Remove first word ' and'
            query.Remove(0, 4);

            return query.ToString();
        }

        #endregion Public methods

        #region Private methods

        /// <summary> 
        /// Returns the request initializer required for authorized requests. 
        /// </summary>
        private async Task<BaseClientService.Initializer> GetInitializerFor(string userId)
        {
            var secrets = new ClientSecrets
            {
                ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DriveClientId],
                ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DriveClientSecret]
            };

            var credentialPersistanceStore = GetPersistentCredentialStore();

            var userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets,
                new[] { DriveService.Scope.Drive }, userId, CancellationToken.None, credentialPersistanceStore);

            var initializer = new BaseClientService.Initializer
            {
                HttpClientInitializer = userCredential,
                ApplicationName = ConfigurationManager.AppSettings[AppSettingKeys.DriveApplicationUserAgent]
            };

            return initializer;
        }

        // todo: get server folder path from Db
        /// <summary> 
        /// Returns a persistent data store for user's credentials.
        /// </summary>
        private IDataStore GetPersistentCredentialStore()
        {
           var folderName = Path.Combine(
                @"C:\Users\Ivan\AppData\Roaming",
                ConfigurationManager.AppSettings[AppSettingKeys.DriveUserCredentalsFolder]);
            var serverDataStore = new FileDataStore(folderName, true);
            return serverDataStore;
        }

        #endregion Private methods
    }
}
