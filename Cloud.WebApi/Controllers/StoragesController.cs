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

		// POST api/storages/1/disconnect
		[Route( "{storageId}/disconnect" )]
		[HttpPost]
		public async Task<IHttpActionResult> DisconnectCloud( [FromUri] int storageId ) {
            var storage = UserStoragesRepository.ResolveStorageInstance(storageId);
			await storage.DisconnectAsync(UserId);

			return Ok();
		}
	}
}