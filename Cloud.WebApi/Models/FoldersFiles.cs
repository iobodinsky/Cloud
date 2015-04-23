using System.Collections.Generic;
using Cloud.Common.Interfaces;

namespace Cloud.WebApi.Models
{
    public class FoldersFiles
    {
        public IEnumerable<IFolder> Folders { get; set; }
        public IEnumerable<IFile> Files { get; set; }
    }
}