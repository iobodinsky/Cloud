using System.Linq;
using System.Web.Http;
using Cloud.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace Cloud.WebApi.Controllers
{
    [RoutePrefix("api/cloud")]
    public class StorageController : ApiControllerBase
    {
        // GET api/cloud/test
        [AllowAnonymous]
        [Route("test")]
        public string GetTest()
        {
            return Repository.Get("61b0b62a-fbdd-4d72-9a9f-1d95bc73765b", 30).Name;
        }

        // GET api/cloud
        [Route("")]
        [HttpGet]
        public UserStorage Cloud()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var files = Repository.GetAll(user.Id)
                .Select(file => new UserFileInfo
                {
                    Id = file.FileId,
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