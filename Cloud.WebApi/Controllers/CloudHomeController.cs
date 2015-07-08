using System.Web.Mvc;

namespace Cloud.WebApi.Controllers
{
    public class CloudHomeController : ControllerBase
    {
        // GET cloud
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Cloud()
        {
            return View();
        }
    }
}