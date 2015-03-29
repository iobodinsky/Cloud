using System.IO;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Models
{
    public class UserFile
    {
        public UserFileInfo UserFileInfo { get; set; }
        public FileType FileType { get; set; }
        public Stream Stream { get; set; }
    }
}
