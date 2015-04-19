using System.Collections.Generic;
using Cloud.Common.Types;

namespace Cloud.Common.Interfaces
{
    public interface IFileRepository
    {
        bool Add(string userId, FullUserFile file);

        IFile Get(string userId, int fileId);

        IEnumerable<IFile> GetRootFiles(string userId);

        IEnumerable<IFolder> GetRootFolders(string userId);

        bool UpdateName(string userId, int fileId, string newfileName);

        bool Delete(string userId, int fileId);
    }
}
