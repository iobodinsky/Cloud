﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Repositories.Repositories;
using Cloud.WebApi.Models;
using Cloud.WebApi.Resources;

namespace Cloud.WebApi.Controllers
{
    [RoutePrefix("api/storages")]
    public class StoragesController : ApiControllerBase
    {

        private readonly UserStorageRepository _userStoragesRepository;

        public StoragesController()
        {
            _userStoragesRepository = new UserStorageRepository();
        }

        // GET api/storages
        [Route("")]
        public async Task<IHttpActionResult> GetUserStorages()
        {
            var userStorages = new UserStorages
            {
                Connected = await Task.Run(() =>
                    _userStoragesRepository.GetConnectedUserStorages(UserId)
                        .Select(storage => new StorageModel
                        {
                            Id = storage.Id,
                            Name = storage.Name,
                            Alias = storage.Alias
                        })),
                Available = await Task.Run(() =>
                    _userStoragesRepository.GetAvailableUserStorages(UserId)
                        .Select(storage => new StorageModel
                        {
                            Id = storage.Id,
                            Name = storage.Name,
                            Alias = storage.Alias
                        })),
            };

            return Ok(userStorages);
        }

        // GET api/storages/dropbox/authorize
        [Route("dropbox/authorize")]
        [HttpGet]
        public async Task<IHttpActionResult> AuthoriseDropbox(
            [FromUri] string code = null, [FromUri] string error = null)
        {
            if (error != null) return RedirectToRoute(Routes.Default, null);

            var storage = StorageFactory.GetDropboxInstance();

            if (string.IsNullOrEmpty(code))
                return Ok(await storage.GetAuthorizationRegirectUrlAsync());

            await storage.AuthorizeAsync(UserId, code);

            return RedirectToRoute(Routes.Default, null);
        }

        // GET api/storages/googledrive/authorize
        [Route("googledrive/authorize")]
        [HttpGet]
        public async Task<IHttpActionResult> AuthoriseGoogleDrive(
            [FromUri] string code = null, [FromUri] string error = null)
        {
            var storage = StorageFactory.GetGoogleDriveInstance();
            await storage.AuthorizeAsync(UserId, code);

            return Ok();
        }

        // POST api/storages/1/disconnect
        [Route("{storageAlias}/disconnect")]
        [HttpPost]
        public async Task<IHttpActionResult> DisconnectCloud([FromUri] string storageAlias)
        {
            var storage = StorageFactory.ResolveInstance(storageAlias);
            await storage.DisconnectAsync(UserId);

            return Ok();
        }
    }
}