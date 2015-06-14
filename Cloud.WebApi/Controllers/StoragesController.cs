using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Common.Models;
using Cloud.Repositories.Repositories;
using Cloud.WebApi.Resources;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/storages" )]
	public class StoragesController : ApiControllerBase {
		private readonly UserStoragesRepository _userStoragesRepository;

		public StoragesController() {
			_userStoragesRepository = new UserStoragesRepository();
		}

		// GET api/storages/available
		[Route( "available" )]
		[AllowAnonymous]
		public async Task<IHttpActionResult> GetAvailableUserStorages() {
			var storages = await Task.Run(() =>
				_userStoragesRepository.GetAvailableUserStorages("baba2553-f024-4afb-aa8d-358b9e1ebf4a"));

			return Ok(storages);
		}

		// GET api/storages/dropbox/authorize
		[Route( "dropbox/authorize" )]
		[HttpGet]
		public async Task<IHttpActionResult> AuthoriseDropbox(
			[FromUri] string code = null, [FromUri] string error = null ) {
			if (error != null) {
				return RedirectToRoute(Routes.Default, null);
			}

			var dropboxStorage = _userStoragesRepository
				.ResolveStorageInstance(Constants.DropboxStorageId);
			await dropboxStorage.AuthorizeAsync(UserId, code);
			return RedirectToRoute(Routes.Default, null);
		}
	}
}