using System.Collections.Generic;
using System.Web;

namespace Cloud.Web.Models
{
    public class UserStorageViewModel
    {
        public UserViewModel UserInfo { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<HttpPostedFileBase> UploadFiles { get; set; }
    }
}