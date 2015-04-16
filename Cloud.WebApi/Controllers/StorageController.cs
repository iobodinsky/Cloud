using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Cloud.Repositories.Models;
using Cloud.Repositories.Repositories;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using UserFile = Cloud.WebApi.Models.UserFile;

namespace Cloud.WebApi.Controllers
{
    //[Authorize]
    public class StorageController : ApiController
    {
        private readonly UserFileRepository _repository;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager != null) return _userManager;

                _userManager = HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
                return _userManager;
            }
        }

        public StorageController()
        {
            _repository = new UserFileRepository();
        }

        public string GetTest()
        {
            return _repository.GetFile("61b0b62a-fbdd-4d72-9a9f-1d95bc73765b", 30).Name;
        }

        // GET api/cloud
        //[Route("/cloud")]
        public IHttpActionResult Cloud()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var files = _repository.GetFiles(user.Id)
                .Select(file => new UserFile
                {
                    Id = file.FileId,
                    Name = file.Name
                });
            var userInfo = new UserInfo { Name = user.UserName };

            var model = new UserStorage
            {
                UserInfo = userInfo,
                Files = files
            };

            // todo:
            return new BadRequestErrorMessageResult("asdas", this);
        }

        // POST api/upload
        [HttpPost]
        public void Upload(HttpPostedFileBase uploadFile)
        {
            if (uploadFile == null) return;

            var userFile = new Repositories.DataContext.UserFile
            {
                Name = uploadFile.FileName,
                AddedDateTime = DateTime.Now,
                IsEditable = true,
                UserId = User.Identity.GetUserId(),
                Size = uploadFile.ContentLength,
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
                Stream = uploadFile.InputStream
            };

            _repository.AddFile(userFileModel);
        }

        // POST api/rename
        [HttpPost]
        public void RenameFile(int fileId, string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return;

            var userId = User.Identity.GetUserId();
            _repository.UpdateFileName(userId, fileId, newFileName);
        }

        // POST api/delete
        //[Route("")]
        [HttpPost]
        public void DeleteFile(int fileId)
        {
            _repository.DeleteFile(User.Identity.GetUserId(), fileId);
        }
    }
}