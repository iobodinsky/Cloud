using System.Net.Http;
using System.Web.Mvc;

namespace Cloud.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
             using (var httpClient = new HttpClient()) 
             {
                 var response = httpClient.GetStringAsync("http://localhost:19800/api/values/");   
                 var s = response.Result;   
             }
         
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}