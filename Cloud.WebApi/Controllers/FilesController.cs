using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Models;
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
        public IEnumerable<UserFileInfo> GetUserFiles()
        {
            var files = Repository.GetFiles(User.Identity.GetUserId())
                .Select(file => new UserFileInfo
                {
                    Id = file.FileId,
                    Name = file.Name
                });

            return files;
        }

        // GET api/files/1
        [Route("{fileId:int:min(1)}")]
        public UserFileInfo GetUserFile(int fileId)
        {
            var file = Repository.GetFile(User.Identity.GetUserId(), fileId);
            var userFile = new UserFileInfo
            {
                Id = file.FileId,
                Name = file.Name
            };

            return userFile;
        }

        // todo: upload file
        // POST api/upload
        [Route("upload")]
        [HttpPost]
        public void UploadFile([FromBody] HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile == null) return;

            var userFile = new UserFile
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

            Repository.AddFile(userFileModel);
        }

        // POST api/rename
        [Route("rename")]
        [HttpPost]
        public void RenameFile([FromBody] int fileId, [FromBody] string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return;

            var userId = User.Identity.GetUserId();
            Repository.UpdateFileName(userId, fileId, newFileName);
        }

        // DELETE api/delete
        [Route("{fileId:int:min(1)}")]
        [HttpDelete]
        public void DeleteFile([FromBody] int fileId)
        {
            Repository.DeleteFile(User.Identity.GetUserId(), fileId);
        }
    }
}
