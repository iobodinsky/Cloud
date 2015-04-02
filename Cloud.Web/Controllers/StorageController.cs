using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
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
            var userInfo = new UserViewModel { Name = user.UserName };

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
        public void Upload(IEnumerable<HttpPostedFileBase> uploadFile)
        {
            
        }
    }
}