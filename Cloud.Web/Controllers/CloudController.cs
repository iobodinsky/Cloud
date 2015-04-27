using System.Configuration;
using System.Web.Mvc;
using Cloud.Common.Models;
using Cloud.Common.Resources;
using Cloud.Web.Resources;
using Cloud.Web.RestService;
using Newtonsoft.Json;
using RestSharp;

namespace Cloud.Web.Controllers
{
    public class CloudController : ControllerBase
    {
        public CloudController(WebApiClient apiClient)
            : base(apiClient)
        {
            
        }

        // GET: Cloud/Index
        [HttpGet]
        [Route("cloud")]
        public ActionResult Index()
        {
            var serverUrl = ConfigurationManager.AppSettings[ServerUrls.Home];
            var authorizationToken = Request.Headers[HttpHeaders.Authorization];
            var responceContent = ApiClient.CallApiWithoutCache(serverUrl, Method.GET, null, authorizationToken);

            var error = JsonConvert.DeserializeObject<ResponceError>(responceContent);
            if (error != null )
            {
                return RedirectToAction("Login");
            }

            var model = JsonConvert.DeserializeObject<UserStorage>(responceContent);
            return View(model);
        }

        // GET: Cloud/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View("");
        }
    }
}