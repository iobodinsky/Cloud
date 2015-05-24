using System.Web.Http;
using Cloud.Common.Managers;
using Cloud.Storages.DataContext;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/folders" )]
	public class FoldersController : ApiControllerBase {

		// GET api/folders/1/cloud/1
		[Route( "{folderId}/cloud/{cloudId:int:min(0)}" )]
		[HttpGet]
		public IHttpActionResult GetFolderData([FromUri] string folderId, 
			[FromUri] int cloudId) {
			var userId = User.Identity.GetUserId();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			var folderDada = cloud.GetFolderData(userId, folderId);

			return Ok(folderDada);
		}

		// POST: api/folders/cloud/1/create
		[Route( "cloud/{cloudId:int:min(0)}/create" )]
		[HttpPost]
		public IHttpActionResult Create([FromUri] int cloudId, [FromBody] UserFolder folder ) {
			var userId = User.Identity.GetUserId();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			var folderId = new IdGenerator().ForFolder();
			folder.Id = folderId;
			folder.UserId = userId;
			folder.CloudId = 2;
			cloud.AddFolder(userId, folder);

			return Ok(folder);
		}

		// DELETE: api/folders/1/cloud/1/delete
		[Route( "{folderId}/cloud/{cloudId:int:min(0)}/delete" )]
		[HttpDelete]
		public IHttpActionResult Delete( [FromUri] string folderId, [FromUri] int cloudId ) {
			var userId = User.Identity.GetUserId();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			cloud.DeleteFolder(userId, folderId);

			// todo: better return type
			return Ok(folderId);
		}
	}
}