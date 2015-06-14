using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Common.Models;
using Cloud.Repositories.Repositories;
using Cloud.WebApi.Models;
using Cloud.WebApi.Resources;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/storages" )]
	public class StoragesController : ApiControllerBase {

		private readonly UserStoragesRepository _userStoragesRepository;

		public StoragesController() {
			_userStoragesRepository = new UserStoragesRepository();
		}

		// GET api/storages
		[Route( "" )]
		public async Task<IHttpActionResult> GetUserStorages() {
			var userStorages = new UserStorages {
				Connected = await Task.Run(() =>
					_userStoragesRepository.GetConnectedUserStorages(UserId)
						.Select(storage => new StorageModel {
							Id = storage.Id,
							Name = storage.Name
						})),
				Available = await Task.Run(() =>
					_userStoragesRepository.GetAvailableUserStorages(UserId)
						.Select(storage => new StorageModel {
							Id = storage.Id,
							Name = storage.Name
						})),
			};

			return Ok(userStorages);
		}

		// POST api/storages/authorize/cloud
		[Route( "authorize/cloud" )]
		[HttpPost]
		public async Task<IHttpActionResult> AuthoriseCloud(
			[FromUri] string code = null, [FromUri] string error = null ) {
			var storage = _userStoragesRepository
				.ResolveStorageInstance(Constants.LocalLenovoStorageId);
			await storage.AuthorizeAsync(UserId, code);

			// todo: successed 
			return Ok("successed outhorised");
		}

		// GET api/storages/authorize/dropbox
		[Route( "authorize/dropbox" )]
		[HttpGet]
		public async Task<IHttpActionResult> AuthoriseDropbox(
			[FromUri] string code = null, [FromUri] string error = null ) {
			if (error != null) {
				return RedirectToRoute(Routes.Default, null);
			}

			var storage = _userStoragesRepository
				.ResolveStorageInstance(Constants.DropboxStorageId);
			await storage.AuthorizeAsync(UserId, code);

			return RedirectToRoute(Routes.Default, null);
		}

		// GET api/storages/authorize/googledrive
		[Route( "authorize/googledrive" )]
		[HttpGet]
		public async Task<IHttpActionResult> AuthoriseGoogleDrive(
			[FromUri] string code = null, [FromUri] string error = null ) {
			var storage = _userStoragesRepository
				.ResolveStorageInstance(Constants.GoogleDriveStorageId);
			await storage.AuthorizeAsync(UserId, code);

			return Ok();
		}

		// POST api/storages/disconnect/cloud
		[Route("disconnect/cloud")]
		[HttpPost]
		public async Task<IHttpActionResult> DisconnectCloud(
			[FromUri] string code = null, [FromUri] string error = null) {
			throw new NotImplementedException();
		}

		// GET api/storages/disconnect/dropbox
		[Route("disconnect/dropbox")]
		[HttpGet]
		public async Task<IHttpActionResult> DisconnectDropbox(
			[FromUri] string code = null, [FromUri] string error = null) {
				throw new NotImplementedException();
		}

		// GET api/storages/disconnect/googledrive
		[Route("disconnect/googledrive")]
		[HttpGet]
		public async Task<IHttpActionResult> DisconnectGoogleDrive(
			[FromUri] string code = null, [FromUri] string error = null) {
				throw new NotImplementedException();
		}
	}
}