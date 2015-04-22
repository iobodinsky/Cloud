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
        public UserStorage GetTest()
        {
            var s = Repository.GetRootFiles("61b0b62a-fbdd-4d72-9a9f-1d95bc73765b");
            var model = new UserStorage
            {
                Files = s.Select(file => new UserFile
                {
                    Id = 4,
                    Name = file.Name
                }),
                UserInfo = new UserInfo
                {
                    Name = "Addddsdsd"
                }
            };
            return model;
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