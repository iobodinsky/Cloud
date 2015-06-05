using System.Collections.Generic;
using System.Web.Http;
using Cloud.Common.Managers;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/folders" )]
	public class FoldersController : ApiControllerBase {
		// GET api/folders/root
		[Route("root")]
		public IHttpActionResult GetRootFolderData() {
			var userId = User.Identity.GetUserId();
			var clouds = new StorageManager().GetStorages();
			var folderDatas = new List<FolderData>();
			foreach (var cloud in clouds) {
				folderDatas.Add(cloud.GetRootFolderData(userId));
			}

			return Ok(folderDatas);
		}

		// GET api/folders/1/cloud/1
		[Route( "{folderId}/cloud/{cloudId:int:min(0)}" )]
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
		public IHttpActionResult Delete( [FromUri] string folderId, [FromUri] int cloudId ) {
			var userId = User.Identity.GetUserId();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			cloud.DeleteFolder(userId, folderId);

			// todo: better return type
			return Ok(folderId);
		}
	}
}