using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Cloud.Common.Resources;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers
{
    [RoutePrefix("api/cloud")]
    public class CloudController : ApiControllerBase
    {
        // GET api/cloud/test
        [AllowAnonymous]
        [Route("{fileId}/cloud/{cloudId:int:min(0)}/download/url")]
        public HttpResponseMessage GetTest([FromUri] string fileId, [FromUri] int cloudId)
        {
            var resporseResult = new HttpResponseMessage(HttpStatusCode.OK);
            var userId = "61b0b62a-fbdd-4d72-9a9f-1d95bc73765b";
            var file = Repository.GetFile(userId, 2, fileId);
            resporseResult.Content = new StreamContent(file.Stream);
            resporseResult.Content.Headers.ContentType = new MediaTypeHeaderValue(InternetMediaTypes.AppStreem);
            return resporseResult;
        }

        // GET api/cloud
        [Route("")]
        [HttpGet]
        public UserStorage Cloud()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var files = Repository.GetRootFiles(user.Id)
                .Select(file => new UserFile
                {
                    Id = file.Id,
                    Name = file.Name
                });
            var userInfo = new UserInfo {Name = user.UserName};

            var userStorage = new UserStorage
            {
                UserInfo = userInfo,
                Files = files
            };

            return userStorage;
        }
    }
}