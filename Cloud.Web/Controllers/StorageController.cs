using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Cloud.Web.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Cloud.Web.Controllers
{
    [Authorize]
    public class StorageController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: Storage/Index
        public ActionResult Index()
        {
            return View(new UserStorageViewModel());
        }

        // POST: Storage/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Upload(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
        }

        // POST: Storage/RenameFile
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public void RenameFile(int fileId, string fileName)
        {

        }

        // POST: Storage/DeleteFile
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public void DeleteFile(int fileId)
        {
        }
    }
}