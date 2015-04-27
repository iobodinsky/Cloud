using System.Web.Mvc;
using Cloud.Web.RestService;

namespace Cloud.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected readonly WebApiClient ApiClient;

        public ControllerBase(WebApiClient apiClient)
        {
            ApiClient = apiClient;
        }
    }
}