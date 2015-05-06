using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiControllerBase
    {
        // GET api/files
        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public FoldersFiles GetRootFoldersFiles()
        {
            //var userId = User.Identity.GetUserId();
            var userId = "61b0b62a-fbdd-4d72-9a9f-1d95bc73765b";
            var model = new FoldersFiles
            {
                Folders = Repository.GetRootFolders(userId).ToList(),
                Files = Repository.GetRootFiles(userId)
            };

            return model;
        }

        #region Download file

        // From Cloud
        // GET api/files/1/cloud/1/download/
        [AllowAnonymous]
        [Route("{fileId}/cloud/{cloudId:int:min(0)}/download/url")]
        [HttpGet]
        public HttpResponseMessage DownloadFile([FromUri] string fileId, [FromUri] int cloudId)
        {
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
        [Route("{fileId}/cloud/{cloudId:int:min(0)}/download/url")]
        public HttpResponseMessage DownloadFile([FromUri] string fileId, [FromUri] int cloudId, [FromUri] string url)
        {
            throw new Exception();
        }

        #endregion Download file

        // POST api/files/cloud/1/upload
        [Route("cloud/{cloudId:int:min(0)}/upload")]
        [HttpPost]
        public HttpResponseMessage UploadFile([FromUri] int cloudId)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                }
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            
            return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            //var userFile = new Storages.DataContext.UserFile
            //{
            //    Name = uploadedFile.FileName,
            //    AddedDateTime = DateTime.Now,
            //    IsEditable = true,
            //    UserId = User.Identity.GetUserId(),
            //    Size = uploadedFile.ContentLength,
            //    DownloadedTimes = 0,
            //    LastModifiedDateTime = DateTime.Now,
            //    // todo: implement directories
            //    Path = string.Format("path")
            //    // todo: implement creation file type
            //    //TypeId = 
            //};

            //var userFileModel = new FullUserFile
            //{
            //    UserFile = userFile,
            //    Stream = uploadedFile.InputStream
            //};

            //Repository.Add(User.Identity.GetUserId(), cloudId, userFileModel);
        }

        // POST api/files/1/cloud/1/rename
        [Route("{fileId}/cloud/{cloudId:int:min(0)}/rename")]
        [HttpPost]
        public void RenameFile([FromUri] string fileId, [FromUri] int cloudId, [FromBody] string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return;

            var userId = User.Identity.GetUserId();
            Repository.UpdateName(userId, cloudId, fileId, newFileName);
        }

        // DELETE api/files/1/cloud/1/delete
        [Route("{fileId}/cloud/{cloudId:int:min(1)}/delete")]
        [HttpDelete]
        public void DeleteFile([FromUri] string fileId, [FromUri] int cloudId)
        {
            Repository.Delete(User.Identity.GetUserId(), cloudId, fileId);
        }
    }
}
