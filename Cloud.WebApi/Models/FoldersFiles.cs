using System.Collections.Generic;
using System.Runtime.Serialization;
using Cloud.Common.Interfaces;

namespace Cloud.WebApi.Models
{
    [CollectionDataContract]
    public class FoldersFiles
    {
        public IEnumerable<IFolder> Folders { get; set; }
        public IEnumerable<IFile> Files { get; set; }
    }
}