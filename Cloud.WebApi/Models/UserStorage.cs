using System.Collections.Generic;

namespace Cloud.WebApi.Models
{
    public class UserStorage
    {
        public UserInfo UserInfo { get; set; }
        public IEnumerable<UserFile> Files { get; set; }
    }
}