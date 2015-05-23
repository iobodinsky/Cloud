using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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
		public FoldersFiles GetRootFoldersFiles() {
			var userId = User.Identity.GetUserId();
			var model = new FoldersFiles {
				Folders = FileRepository.GetRootFolders(userId).ToList(),
				Files = FileRepository.GetRootFiles(userId),
				CurrentFolderId = FileRepository.GetRootFolderId(userId)
			};

			return model;
		}

		#region Download file

		// From Cloud
		// GET api/files/1/cloud/1/download/
		[AllowAnonymous]
		[Route( "{fileId}/cloud/{cloudId:int:min(0)}/download/url" )]
		[HttpGet]
		public HttpResponseMessage DownloadFile( [FromUri] string fileId, [FromUri] int cloudId ) {
			throw new NotImplementedException();
			//var resporseResult = new HttpResponseMessage(HttpStatusCode.OK);
			//var userId = "61b0b62a-fbdd-4d72-9a9f-1d95bc73765b";
			//var file = Repository.GetFile(userId, cloudId, fileId);
			//resporseResult.Content = new StreamContent(file.Stream);
			//resporseResult.Content.Headers.ContentType = new MediaTypeHeaderValue(InternetMediaTypes.AppStreem);
			//return resporseResult;
		}

		//From Drive
		// GET api/files/1/cloud/1/download/
		[Route( "{fileId}/cloud/{cloudId:int:min(0)}/download/url" )]
		public HttpResponseMessage DownloadFile( [FromUri] string fileId, [FromUri] int cloudId, [FromUri] string url ) {
			throw new Exception();
		}

		#endregion Download file

		// POST api/files/cloud/1/folder/1/upload
		[Route( "cloud/{cloudId:int:min(0)}/folder/{folderId}/upload" )]
		[HttpPost]
		public HttpResponseMessage UploadFile( [FromUri] int cloudId,
			[FromUri] string folderId ) {
			var httpRequest = HttpContext.Current.Request;

			foreach (string file in httpRequest.Files) {
				var postedFile = httpRequest.Files[file];
				if (postedFile == null) continue;

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

				FileRepository.Add(User.Identity.GetUserId(), cloudId, userFileModel);
			}

			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		// POST api/files/1/cloud/1/rename
		[Route( "{fileId}/cloud/{cloudId:int:min(0)}/rename" )]
		[HttpPost]
		public void RenameFile( [FromUri] string fileId, [FromUri] int cloudId, 
			[FromBody] NewFolderModel newfile ) {
				if (string.IsNullOrEmpty(newfile.Name)) return;
			var userId = User.Identity.GetUserId();
			FileRepository.UpdateName(userId, cloudId, fileId, newfile.Name);
		}

		// DELETE api/files/1/cloud/1/delete
		[Route( "{fileId}/cloud/{cloudId:int:min(1)}/delete" )]
		[HttpDelete]
		public void DeleteFile( [FromUri] string fileId, [FromUri] int cloudId ) {
			FileRepository.Delete(User.Identity.GetUserId(), cloudId, fileId);
		}
	}
}
