using System.IO;
using Cloud.Common.Interfaces;

namespace Cloud.Common.Types
{
    public class FullUserFile
    {
        public IFile UserFile { get; set; }
        public Stream Stream { get; set; }
    }
}
