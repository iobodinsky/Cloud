using System.Collections.Generic;
using Cloud.Common.Models;

namespace Cloud.Common.Interfaces
{
    public interface IFileRepository
    {
        bool Add(string userId, int cloudId, FullUserFile file);

        IFile GetFileInfo(string userId, int cloudId, string fileId);

        FullUserFile GetFile(string userId, int cloudId, string fileId);

        IEnumerable<IFile> GetRootFiles(string userId);

        IEnumerable<IFolder> GetRootFolders(string userId);

        bool UpdateName(string userId, int cloudId, string fileId, string newfileName);

        bool Delete(string userId, int cloudId, string fileId);
    }
}
