using System.Web.Http;
using Cloud.Storages.DataContext;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/folders" )]
	public class FoldersController : ApiControllerBase {

		// POST: api/folders/create
		[Route("create")]
		[HttpPost]
		public IHttpActionResult Create([FromBody] UserFolder folder) {
			var userId = User.Identity.GetUserId();
			// todo:
			var cloudId = 2;
			folder.UserId = userId;
			FolderRepository.Add(userId, cloudId, folder);

			return Ok();
		}
	}
}
