using System.Web;
using System.Web.Http;
using Cloud.Repositories.Repositories;
using Cloud.WebApi.ActionFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Cloud.WebApi.Controllers {
	[Authorize]
	[ExceptionFilter]
	public abstract class ApiControllerBase : ApiController {
		private ApplicationUserManager _userManager;

		protected readonly string UserId;
		protected readonly StorageRepository StorageRepository;

		public ApplicationUserManager UserManager {
			get {
				if (_userManager != null) return _userManager;

				_userManager = HttpContext.Current.GetOwinContext()
					.GetUserManager<ApplicationUserManager>();
				return _userManager;
			}
		}

		protected ApiControllerBase() {
			UserId = User.Identity.GetUserId();
			StorageRepository = new StorageRepository();
		}

		protected ApiControllerBase( ApplicationUserManager userManager ) {
			_userManager = userManager;
			UserId = User.Identity.GetUserId();
			StorageRepository = new StorageRepository();
		}
	}
}