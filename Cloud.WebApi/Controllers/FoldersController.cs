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

			return Ok(folder);
		}

		// DELETE: api/folders/1/cloud/1/delete
		[Route("{folderId}/cloud/{cloudId:int:min(0)}/delete")]
		[HttpDelete]
		public IHttpActionResult Delete( [FromUri] string folderId, [FromUri] int cloudId ) {
			var userId = User.Identity.GetUserId();
			FolderRepository.Delete(userId, cloudId, folderId);

			// todo: better return type
			return Ok(folderId);
		}
	}
}
