using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Models;
using Cloud.Repositories.Repositories;
using Cloud.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Cloud.Web.Controllers
{
    [Authorize]
    public class StorageController : Controller
    {
        private readonly UserFileRepository _repository;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public StorageController()
        {
            _repository = new UserFileRepository();
        }

        // GET: Storage/Index
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var files = _repository.GetFiles(user.Id)
                .Select(file => new FileViewModel
                {
                    Id = file.FileId,
                    Name = file.Name
                });
            var userInfo = new UserViewModel {Name = user.UserName};

            var model = new UserStorageViewModel
            {
                UserInfo = userInfo,
                Files = files
            };

            return View(model);
        }

        // POST: Storage/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Upload(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null) return;

            foreach (var uploadFile in uploadFiles)
            {
                var userFile = new UserFile
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
        }

        // POST: Storage/RenameFile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void RenameFile(int fileId, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            var userId = User.Identity.GetUserId();
            _repository.UpdateFileName(userId, fileId, fileName);
        }

        // POST: Storage/DeleteFile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeleteFile(int fileId)
        {
            _repository.DeleteFile(User.Identity.GetUserId(), fileId);
        }
    }
}