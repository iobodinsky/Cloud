using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using Cloud.Common.Managers;
using Cloud.Common.Models;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers {
	[RoutePrefix( "api/files" )]
	public class FilesController : ApiControllerBase {
		// GET api/files
		[Route( "" )]
		[HttpGet]
		public IHttpActionResult GetRootFolderData() {
			var userId = User.Identity.GetUserId();

			var rootFolder = StorageRepository.GetRootFolder(userId);
			var model = new FolderData {
				Folders = StorageRepository.GetRootFolders(userId).ToList(),
				Files = StorageRepository.GetRootFiles(userId),
				Folder = rootFolder
			};

			return Ok(model);
		}

		// GET api/files/1/cloud/1/download/
		[AllowAnonymous]
		[Route( "{fileId}/cloud/{cloudId:int:min(0)}/download/url" )]
		[HttpGet]
		public IHttpActionResult DownloadFile( [FromUri] string fileId,
			[FromUri] int cloudId ) {
			throw new NotImplementedException();
			//var resporseResult = new HttpResponseMessage(HttpStatusCode.OK);
			//var userId = "61b0b62a-fbdd-4d72-9a9f-1d95bc73765b";
			//var file = Repository.GetFile(userId, cloudId, fileId);
			//resporseResult.Content = new StreamContent(file.Stream);
			//resporseResult.Content.Headers.ContentType = new MediaTypeHeaderValue(InternetMediaTypes.AppStreem);
			//return resporseResult;
		}

		// GET api/files/1/cloud/1/download/
		[Route( "{fileId}/cloud/{cloudId:int:min(0)}/download/url" )]
		public IHttpActionResult DownloadFile( [FromUri] string fileId,
			[FromUri] int cloudId, [FromUri] string url ) {
			throw new Exception();
		}

		// POST api/files/cloud/1/folder/1/upload
		[Route( "cloud/{cloudId:int:min(0)}/folder/{folderId}/upload" )]
		[HttpPost]
		public IHttpActionResult UploadFile( [FromUri] int cloudId,
			[FromUri] string folderId ) {
			var httpRequest = HttpContext.Current.Request;

			var postedFile = httpRequest.Files.Get(0);
			var userFile = new Storages.DataContext.UserFile {
				Id = new IdGenerator().ForFile(),
				Name = postedFile.FileName,
				AddedDateTime = DateTime.Now,
				IsEditable = true,
				UserId = User.Identity.GetUserId(),
				Size = postedFile.ContentLength,
				DownloadedTimes = 0,
				LastModifiedDateTime = DateTime.Now,
				FolderId = folderId,
			};

			var userFileModel = new FullUserFile {
				UserFile = userFile,
				Stream = postedFile.InputStream
			};

			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			cloud.AddFile(User.Identity.GetUserId(), userFileModel);

			return Ok(userFile);
		}

		// POST api/files/1/cloud/1/rename
		[Route( "{fileId}/cloud/{cloudId:int:min(0)}/rename" )]
		[HttpPost]
		public IHttpActionResult RenameFile( [FromUri] string fileId, [FromUri] int cloudId,
			[FromBody] NewFileModel newfile ) {
			if (string.IsNullOrEmpty(newfile.Name)) return BadRequest();
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			cloud.UpdateName(User.Identity.GetUserId(), fileId, newfile.Name);

			return Ok();
		}

		// DELETE api/files/1/cloud/1/delete
		[Route( "{fileId}/cloud/{cloudId:int:min(1)}/delete" )]
		[HttpDelete]
		public IHttpActionResult DeleteFile( [FromUri] string fileId, [FromUri] int cloudId ) {
			var cloud = StorageRepository.ResolveStorageInstance(cloudId);
			cloud.DeleteFile(User.Identity.GetUserId(), fileId);

			return Ok();
		}
	}
}
