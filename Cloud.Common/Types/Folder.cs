using Cloud.Common.Interfaces;

namespace Cloud.Common.Types
{
    public class Folder : IFolder
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
