using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Cloud.Common.Managers;
using Cloud.Common.Models;
using Cloud.Common.Resources;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;
using HttpHeaders = Cloud.Common.Resources.HttpHeaders;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/files" )]
	public class FilesController : ApiControllerBase {
		// GET api/files/1/requestlink
		[Route( "{fileId}/requestlink" )]
		[HttpGet]
		public async Task<IHttpActionResult> RequestDownloadFileLink(
			[FromUri] string fileId ) {
			var downloadUrl = await Task.Run(() => {
				var userId = User.Identity.GetUserId();
				return ConstractCloudDownloadFileUrl(fileId, userId);
			});

			return Ok(downloadUrl);
		}

		// GET api/files/1/users/1/download
		[Route( "{fileId}/users/{userId}/download" )]
		[HttpGet]
		[AllowAnonymous]
		public HttpResponseMessage DownloadFile( [FromUri] string fileId,
			[FromUri] string userId ) {
			var resporseResult = new HttpResponseMessage(HttpStatusCode.OK);
			var file = StorageRepository.GetFullFile(userId, fileId);
			resporseResult.Content = new StreamContent(file.Stream);
			resporseResult.Content.Headers.ContentType =
				new MediaTypeHeaderValue(InternetMediaTypes.AppStreem);
			resporseResult.Content.Headers.ContentDisposition =
				new ContentDispositionHeaderValue(HttpHeaders.ContentDispositionAttachment) {
					FileName = file.UserFile.Name
				};

			return resporseResult;
		}

		// POST api/files/cloud/1/folder/1/upload
		[Route( "cloud/{cloudId:int}/folder/{folderId}/upload" )]
		[HttpPost]
		public async Task<IHttpActionResult> UploadFile( [FromUri] int cloudId,
			[FromUri] string folderId ) {

			var httpRequest = HttpContext.Current.Request;

			var postedFile = httpRequest.Files.Get(0);
			var file = new Repositories.DataContext.UserFile {
				Id = new IdGenerator().ForFile(),
				Name = postedFile.FileName,
				AddedDateTime = DateTime.Now,
				IsEditable = true,
				UserId = User.Identity.GetUserId(),
				Size = postedFile.ContentLength,
				DownloadedTimes = 0,
				LastModifiedDateTime = DateTime.Now,
				FolderId = folderId,
				CloudId = 2
			};

			var userFileModel = new FullUserFile {
				UserFile = file,
				Stream = postedFile.InputStream
			};

			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			var createdFile = await cloud.AddFileAsync(
				User.Identity.GetUserId(), userFileModel);

			return Ok(createdFile);
		}

		// POST api/files/1/cloud/1/rename
		[Route( "{fileId}/cloud/{cloudId:int}/rename" )]
		[HttpPost]
		public async Task<IHttpActionResult> RenameFile( [FromUri] string fileId,
			[FromUri] int cloudId, [FromBody] NewNameModel newfile ) {
			if (string.IsNullOrEmpty(newfile.Name)) {
				// todo:
				return BadRequest();
			}
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			var newFileName = await cloud.UpdateFileNameAsync(User.Identity.GetUserId(), fileId, newfile.Name);

			return Ok(newFileName);
		}

		// DELETE api/files/1/cloud/1/delete
		[Route( "{fileId}/cloud/{cloudId:int:min(1)}/delete" )]
		[HttpDelete]
		public async Task<IHttpActionResult> DeleteFile( [FromUri] string fileId,
			[FromUri] int cloudId ) {
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			var result = await cloud.DeleteFileAsync(User.Identity.GetUserId(), fileId);

			return Ok(result);
		}

		#region Private methods

		private string ConstractCloudDownloadFileUrl( string fileId, string userId ) {
			return string.Format("api/files/{0}/users/{1}/download", fileId, userId);
		}

		#endregion Private methods
	}
}