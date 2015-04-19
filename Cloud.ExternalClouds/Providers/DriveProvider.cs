using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Common.Types;
using Cloud.ExternalClouds.Models;
using Cloud.ExternalClouds.Resources;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Cloud.ExternalClouds.Providers
{
    public class DriveProvider : IStorage
    {
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
            var filderName = ConfigurationManager.AppSettings[AppSettingKeys.DriveUserCredentalsFolder];
            var serverDataStore = new FileDataStore(filderName);
            return serverDataStore;
        }

        #endregion Private methods

        #region IStorage implementation

        public bool Add(string userId, FullUserFile file)
        {
            throw new NotImplementedException();
        }

        public IFile Get(string userId, int fileId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFile> GetAll(string userId)
        {
            var service = BuildServiceAsync(userId);
            var request = service.Files.List();
            request.MaxResults = 2;

            return request.Execute().Items.Select(file => new DriveFile
            {
                UserId = userId,
                Name = file.Title,
                LastModifiedDateTime = file.LastViewedByMeDate == null
                    ? new DateTime()
                    : file.LastViewedByMeDate.Value,
                AddedDateTime = file.CreatedDate == null
                    ? new DateTime()
                    : file.CreatedDate.Value,
            });
        }

        public bool UpdateName(string userId, int fileId, string newfileName)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string userId, int fileId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFile> GetRootFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFile> GetFilesIn(string folder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Folder> GetRootFolders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Folder> GetFoldersIn(string folder)
        {
            throw new NotImplementedException();
        }

        #endregion IStorage implementation
    }
}