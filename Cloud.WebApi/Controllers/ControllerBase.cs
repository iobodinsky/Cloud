using System.Web;
using System.Web.Mvc;
using Cloud.Repositories.Repositories;
using Cloud.WebApi.ActionFilters;
using Microsoft.AspNet.Identity.Owin;

namespace Cloud.WebApi.Controllers
{
    [ExceptionFilter]
    public class ControllerBase : Controller
    {
        private ApplicationUserManager _userManager;

        protected readonly UserStoragesRepository UserStoragesRepository;

        protected ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager != null) return _userManager;

                _userManager = HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
                return _userManager;
            }
        }

        protected ControllerBase()
        {
            UserStoragesRepository = new UserStoragesRepository();
        }
    }
}