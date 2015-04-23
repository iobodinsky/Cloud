using System;
using System.Web;
using System.Web.Http;
using Cloud.Common.Types;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiControllerBase
    {
        // GET api/files
        [Route("")]
        [HttpGet]
        public FoldersFiles GetRootFoldersFiles()
        {
            var userId = User.Identity.GetUserId();
            var model = new FoldersFiles
            {
                Folders = Repository.GetRootFolders(userId),
                Files = Repository.GetRootFiles(userId)
            };

            return model;
        }

        // GET api/files/1/cloud/1/download
        [Route("{fileId}/cloud/{cloudId:int:min(0)}/download")]
        public void DownloadUserFile([FromUri] string fileId, [FromUri] int cloudId)
        {
            throw new NotImplementedException();
            var file = Repository.Get(User.Identity.GetUserId(), cloudId, fileId);
            var userFile = new UserFile
            {
                Id = file.Id,
                Name = file.Name
            };

            //return userFile;
        }

        // POST api/files/cloud/1/upload
        [Route("cloud/{cloudId:int:min(0)}/upload")]
        [HttpPost]
        public void UploadFile([FromUri] int cloudId, [FromBody] HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile == null) return;

            var userFile = new Repositories.DataContext.UserFile
            {
                Name = uploadedFile.FileName,
                AddedDateTime = DateTime.Now,
                IsEditable = true,
                UserId = User.Identity.GetUserId(),
                Size = uploadedFile.ContentLength,
                DownloadedTimes = 0,
                LastModifiedDateTime = DateTime.Now,
                // todo: implement directories
                Path = string.Format("path")
                // todo: implement creation file type
                //TypeId = 
            };

            var userFileModel = new FullUserFile
            {
                UserFile = userFile,
                Stream = uploadedFile.InputStream
            };

            Repository.Add(User.Identity.GetUserId(), cloudId, userFileModel);
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
