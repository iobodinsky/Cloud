using System;
using System.Configuration;
using System.Threading.Tasks;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Repositories;
using Cloud.Storages.Resources;
using DropboxRestAPI;

namespace Cloud.Storages.Dropbox
{
    internal class DropboxManager
    {
        private readonly DropboxUserTokenRepository _tokenRepository;
        private readonly UserStorageRepository _userStoragesRepository;
        private readonly int _dropboxStorageId;

        public DropboxManager(int dropboxStorageId)
        {
            _dropboxStorageId = dropboxStorageId;
            _tokenRepository = new DropboxUserTokenRepository();
            _userStoragesRepository = new UserStorageRepository();
        }

        public async Task AuthorizeAsync(string userId, string code)
        {
            var options = new Options
            {
                ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
                ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
                RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
            };

            var client = new Client(options);

            if (string.IsNullOrEmpty(code))
            {
                var authRequestUrl = await client.Core.OAuth2
                    .AuthorizeAsync(DropboxKeys.AuthorizeResponceType);
                // todo
                throw new Exception("Dropbox account unauthorised",
                    new Exception(authRequestUrl.AbsoluteUri));
            }

            var token = await client.Core.OAuth2.TokenAsync(code);
            var dropboxToken = new DropboxUserToken
            {
                UserId = userId,
                AccessToken = token.access_token,
                TeamId = token.team_id,
                TokenType = token.token_type,
                Uid = token.uid
            };

            await _tokenRepository.AddOrUpdateAsunc(dropboxToken, userId);
            await _userStoragesRepository.AddAsync(userId, _dropboxStorageId);
        }

        public async Task<Client> GetClient(string userId)
        {
            var options = new Options
            {
                ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
                ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
                RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
            };
            var client = new Client(options);

            var userToken = await new DropboxUserTokenRepository()
                .GetTokenAsync(userId);

            if (userToken == null) await AuthorizeAsync(userId, string.Empty);
            else client.UserAccessToken = userToken.AccessToken;

            return client;
        }

        public string ConstructEntityId(string path)
        {
            return path.Replace('/', '|');
        }

        public string ConstructEntityPath(string path)
        {
            return path.Replace('|', '/');
        }

        public string MakeValidPath(string path)
        {
            return path.Replace("\\", "/")
                .Replace("//", "/");
        }

        public async Task DisconnectAsync(string userId, int id)
        {
            await _tokenRepository.DeleteAsync(userId);
            await _userStoragesRepository.DeleteAsync(userId, id);
        }
    }
}