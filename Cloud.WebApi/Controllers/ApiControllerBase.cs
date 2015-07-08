using System.Web;
using System.Web.Http;
using Cloud.Repositories.Repositories;
using Cloud.Storages;
using Cloud.WebApi.ActionFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Cloud.WebApi.Controllers
{
    [Authorize]
    [ExceptionFilter]
    public abstract class ApiControllerBase : ApiController
    {
        private ApplicationUserManager _userManager;

        protected readonly StorageFactory StorageFactory;
        protected readonly string UserId;
        protected readonly UserStorageRepository UserStoragesRepository;

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
            StorageFactory = new StorageFactory();
            UserId = User.Identity.GetUserId();
            UserStoragesRepository = new UserStorageRepository();
        }

        protected ApiControllerBase(ApplicationUserManager userManager) : this()
        {
            _userManager = userManager;
        }
    }
}