using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Common.Managers;
using Cloud.Common.Models;
using Cloud.Storages;
using Cloud.Storages.DataContext;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/folders" )]
	public class FoldersController : ApiControllerBase {
		// GET api/folders
		[Route( "" )]
		public async Task<IHttpActionResult> GetRootFolderData() {
			var userId = User.Identity.GetUserId();
			var clouds = new StorageManager().GetStorages();
			var folderDatas = new List<FolderData>();
			foreach (var cloud in clouds) {
				folderDatas.Add(await cloud.GetRootFolderDataAsync(userId));
			}

			return Ok(folderDatas);
		}

		// GET api/folders/1/cloud/1
		[Route( "{folderId}/cloud/{cloudId:int}" )]
		public async Task<IHttpActionResult> GetFolderData( [FromUri] string folderId,
			[FromUri] int cloudId ) {
			var userId = User.Identity.GetUserId();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			var folderDada = await cloud.GetFolderDataAsync(userId, folderId);

			return Ok(folderDada);
		}

		// POST: api/folders/cloud/1/create
		[Route( "cloud/{cloudId:int}/create" )]
		[HttpPost]
		public async Task<IHttpActionResult> Create(
			[FromUri] int cloudId, [FromBody] UserFolder folder ) {
			var userId = User.Identity.GetUserId();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			var folderId = new IdGenerator().ForFolder();
			folder.Id = folderId;
			folder.UserId = userId;
			folder.CloudId = 2;
			var createdFolder = await cloud.AddFolderAsync(userId, folder);

			return Ok(createdFolder);
		}

		// POST api/folders/1/cloud/1/rename
		[Route( "{folderId}/cloud/{cloudId:int}/rename" )]
		[HttpPost]
		public IHttpActionResult RenameFolder( [FromUri] string folderId, [FromUri] int cloudId,
			[FromBody] NewNameModel newFolder ) {
			if (string.IsNullOrEmpty(newFolder.Name)) return BadRequest();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			cloud.UpdateFolderNameAsync(User.Identity.GetUserId(), folderId, newFolder.Name);

			return Ok(newFolder.Name);
		}

		// DELETE: api/folders/1/cloud/1/delete
		[Route( "{folderId}/cloud/{cloudId:int}/delete" )]
		public IHttpActionResult Delete( [FromUri] string folderId, [FromUri] int cloudId ) {
			var userId = User.Identity.GetUserId();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			cloud.DeleteFolderAsync(userId, folderId);

			// todo: better return type
			return Ok(folderId);
		}
	}
}