using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
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
        public IEnumerable<UserFile> GetUserFiles()
        {
            var files = Repository.GetFiles(User.Identity.GetUserId())
                .Select(file => new UserFile
                {
                    Id = file.FileId,
                    Name = file.Name
                });

            return files;
        }

        // GET api/files/1
        [Route("{fileId:int:min(1)}")]
        public UserFile GetUserFile(int fileId)
        {
            var file = Repository.GetFile(User.Identity.GetUserId(), fileId);
            var userFile = new UserFile
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
        public void UploadFile(HttpPostedFileBase uploadedFile)
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

            var userFileModel = new UserFileModel
            {
                UserFile = userFile,
                Stream = uploadedFile.InputStream
            };

            Repository.AddFile(userFileModel);
        }

        // POST api/rename
        [Route("{fileId:int:min(1)}/rename/{newFileName}")]
        [HttpPost]
        public void RenameFile(int fileId, string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return;

            var userId = User.Identity.GetUserId();
            Repository.UpdateFileName(userId, fileId, newFileName);
        }

        // DELETE api/delete
        [Route("{fileId:int:min(1)}")]
        [HttpDelete]
        public void DeleteFile(int fileId)
        {
            Repository.DeleteFile(User.Identity.GetUserId(), fileId);
        }
    }
}
