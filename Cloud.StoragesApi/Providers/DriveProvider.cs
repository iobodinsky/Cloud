using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.StoragesApi.Models;
using Cloud.StoragesApi.Resources;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Cloud.StoragesApi.Providers
{
    public class DriveProvider
    {
        #region Public methods

        // todo: get also file id
        public IEnumerable<IFile> GetFirstTenFilesFor(string userId)
        {
            var service = BuildServiceAsync(userId);
            var request = service.Files.List();
            return request.Execute().Items.Select(file => new DriveFile
            {
                UserId = userId,
                Name = file.Title,
                LastModifiedDateTime = file.LastViewedByMeDate == null ? 
                    new DateTime() : file.LastViewedByMeDate.Value,
                AddedDateTime = file.CreatedDate == null ?
                    new DateTime() : file.CreatedDate.Value,
            })
            .Take(10);
        }

        #endregion Public methods
        
        #region Private methods

        /// <summary>
        /// Returns Drive service
        /// </summary>
        private DriveService BuildServiceAsync(string userId)
        {
            var initializer = GetInitializerFor(userId).Result;
            return new DriveService(initializer);
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
            var filderName = ConfigurationManager.AppSettings[AppSettingKeys.DriveUserCredentalsFolder];
            var serverDataStore = new FileDataStore(filderName);
            return serverDataStore;
        }

        #endregion Private methods
    }
}