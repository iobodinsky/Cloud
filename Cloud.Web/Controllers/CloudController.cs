using System.Web.Mvc;
using Cloud.Web.RestService;

namespace Cloud.Web.Controllers
{
    public class CloudController : ControllerBase
    {
        public CloudController(WebApiClient apiClient)
            : base(apiClient)
        {
            
        }

        // GET: Cloud
        public ActionResult Index()
        {
            return View();
        }
    }
}