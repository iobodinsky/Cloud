using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cloud.StoragesApi.Resources;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Cloud.StoragesApi.Providers
{
    public class DriveProvider
    {
        #region Public methods

        public IEnumerable<File> GetFirstTenFilesFor(string userId)
        {
            var service = BuildServiceAsync(userId);
            var request = service.Files.List();
            return request.Execute().Items.Take(10);
        } 

        #endregion Public methods
        
        #region Private methods

        /// <summary>
        /// Returns Drive service
        /// </summary>
        private DriveService BuildServiceAsync(string userId)
        {
            var credentials = GetInitializerFor(userId).Result;
            return new DriveService(credentials);
        }

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
                new [] { DriveService.Scope.Drive }, userId, CancellationToken.None, credentialPersistanceStore);

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
            var serverDataStore = new FileDataStore("Drive.Sample.Credentals");
            return serverDataStore;
        }

        #endregion Private methods
    }
}