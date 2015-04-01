using System.IO;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Models
{
    public class UserFileModel
    {
        public UserFile UserFile { get; set; }
        public UserFileType FileType { get; set; }
        public Stream Stream { get; set; }
    }
}
