using System.Collections.Generic;

namespace Cloud.WebApi.Models
{
    public class FoldersFiles
    {
        public IEnumerable<UserFolder> Folders { get; set; }
        public IEnumerable<UserFile> Files { get; set; }
    }
}