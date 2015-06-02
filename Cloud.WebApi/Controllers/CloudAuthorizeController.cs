using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Storages.Resources;
using DropboxRestAPI;

namespace Cloud.WebApi.Controllers {
	
	[RoutePrefix("api/clouds")]
	[AllowAnonymous]
	public class CloudAuthorizeController : ApiController {

		// GET api/clouds/dropbox/authorise
		[Route( "dropbox/authorise" )]
		[HttpGet]
		public async Task<IHttpActionResult> Authorise(
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
	}
}