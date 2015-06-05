using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Storages.Resources;
using DropboxRestAPI;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers {

	[RoutePrefix("api/clouds")]
	[AllowAnonymous]
	public class CloudsController : ApiController {

		// GET api/clouds/googledrive/authorize
		[Route("googledrive/authorize")]
		[HttpGet]
		public async Task<IHttpActionResult> AuthoriseGoogleDrive(
			[FromUri] string code, [FromUri] string error) {

			var options = new Options {
				ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
				ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
				RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
			};

			var client = new Client(options);
			var token = await client.Core.OAuth2.TokenAsync(code);

			
			return Ok();
		}

		// GET api/clouds/dropbox/authorize
		[Route("dropbox/authorize")]
		[HttpGet]
		public async Task<IHttpActionResult> AuthoriseDropbox(
			[FromUri] string code = null, [FromUri] string error = null) {

			var options = new Options {
				ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
				ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
				RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
			};

			var client = new Client(options);
			var token = await client.Core.OAuth2.TokenAsync(code);

			var userId = User.Identity.GetUserId();

			return Ok();
		}
	}
}