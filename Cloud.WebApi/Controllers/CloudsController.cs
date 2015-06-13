using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Repositories;
using Cloud.Storages.Resources;
using DropboxRestAPI;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers {

	[RoutePrefix("api/clouds")]
	public class CloudsController : ApiController {
		// GET api/clouds/dropbox/authorize
		[Route("dropbox/authorize")]
		[HttpGet]
		public async Task<IHttpActionResult> AuthoriseDropbox(
			[FromUri] string code = null, [FromUri] string error = null) {
			if (error != null) {
				return RedirectToRoute("Default", null);
			}

			var options = new Options {
				ClientId = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppKey],
				ClientSecret = ConfigurationManager.AppSettings[AppSettingKeys.DropboxAppSecret],
				RedirectUri = ConfigurationManager.AppSettings[AppSettingKeys.DropboxRedirectUri]
			};

			var client = new Client(options);
			var token = await client.Core.OAuth2.TokenAsync(code);
			var userId = User.Identity.GetUserId();
			var dropboxToken = new DropboxUserToken {
				UserId = userId,
				AccessToken = token.access_token,
				TeamId = token.team_id,
				TokenType = token.token_type,
				Uid = token.uid
			};
			var repository = new DropboxUserTokenRepository();
			await repository.AddAsync(dropboxToken, true);

			return RedirectToRoute("Default", null);
		}
	}
}