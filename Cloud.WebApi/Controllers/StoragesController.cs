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
		[Route("")]
		public async Task<IHttpActionResult> GetUserStorages() {
			var userStorages = new UserStorages {
				Connected = await Task.Run(() =>
					_userStoragesRepository.GetConnectedUserStorages(UserId)),
				Available = await Task.Run(() =>
					_userStoragesRepository.GetAvailableUserStorages(UserId))
			};
			
			return Ok(userStorages);
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