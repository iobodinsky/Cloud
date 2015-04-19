using System.Web;
using System.Web.Http;
using Cloud.Repositories;
using Cloud.Repositories.Repositories;
using Microsoft.AspNet.Identity.Owin;

namespace Cloud.WebApi.Controllers
{
    [Authorize]
    public abstract class ApiControllerBase : ApiController
    {
        private ApplicationUserManager _userManager;

        protected readonly IFileRepository Repository;

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

        protected ApiControllerBase()
        {
            Repository = new UserFileRepository();
        }
    }
}