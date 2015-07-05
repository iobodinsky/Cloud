using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Cloud.Common.Managers;
using Cloud.Common.Models;
using Cloud.Storages.Dropbox;
using Cloud.WebApi.Models;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/files" )]
	public class FilesController : ApiControllerBase {
		// GET api/files/1/download/dropbox
		[Route( "{fileId}/download/dropbox" )]
		public async Task<IHttpActionResult> GetDownloadUrlDropbox( [FromUri] string fileId ) {
			var downloadUrl = await new DropboxStorage(Constants.DropboxStorageId)
				.GetDownloadUrl(UserId, fileId);

			return Ok(downloadUrl);
		}

		// POST api/files/cloud/1/folder/1/upload
		[Route( "cloud/{storageId:int}/folder/{folderId}/upload" )]
		[HttpPost]
		public async Task<IHttpActionResult> UploadFile( [FromUri] int storageId,
			[FromUri] string folderId ) {

			var httpRequest = HttpContext.Current.Request;

			var postedFile = httpRequest.Files.Get(0);
			var file = new Repositories.DataContext.UserFile {
				Id = new IdGenerator().ForFile(),
				Name = postedFile.FileName,
				AddedDateTime = DateTime.Now,
				IsEditable = true,
				UserId = UserId,
				Size = postedFile.ContentLength,
				DownloadedTimes = 0,
				LastModifiedDateTime = DateTime.Now,
				FolderId = folderId,
				StorageId = storageId
			};

			var userFileModel = new FullUserFile {
				UserFile = file,
				Stream = postedFile.InputStream
			};

            var cloud = UserStoragesRepository.ResolveStorageInstance(storageId);
			var createdFile = await cloud.AddFileAsync(
				UserId, userFileModel);

			return Ok(createdFile);
		}

		// POST api/files/1/cloud/1/rename
		[Route( "{fileId}/cloud/{storageId:int}/rename" )]
		[HttpPost]
		public async Task<IHttpActionResult> RenameFile( [FromUri] string fileId,
			[FromUri] int storageId, [FromBody] NewNameModel newfile ) {
			if (string.IsNullOrEmpty(newfile.Name)) {
				// todo:
				return BadRequest();
			}
            var cloud = UserStoragesRepository.ResolveStorageInstance(storageId);
			var newFileName = await cloud.UpdateFileNameAsync(UserId, fileId, newfile.Name);

			return Ok(newFileName);
		}

		// DELETE api/files/1/cloud/1/delete
		[Route( "{fileId}/cloud/{storageId:int:min(1)}/delete" )]
		[HttpDelete]
		public async Task<IHttpActionResult> DeleteFile( [FromUri] string fileId,
			[FromUri] int storageId ) {
                var cloud = UserStoragesRepository.ResolveStorageInstance(storageId);
			await cloud.DeleteFileAsync(UserId, fileId);

			return Ok();
		}
	}
}