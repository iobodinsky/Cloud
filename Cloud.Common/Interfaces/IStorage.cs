using System.Collections.Generic;
using Cloud.Common.Models;

namespace Cloud.Common.Interfaces
{
    public interface IStorage
    {
        bool Add(string userId, FullUserFile file);

        IEnumerable<IFile> GetRootFiles(string userId);

        IEnumerable<IFile> GetFilesIn(string userId,string folder);
            
        IEnumerable<IFolder> GetRootFolders(string userId);

        IEnumerable<IFolder> GetFoldersIn(string userId, string folder);

        IFile GetFileInfo(string userId, string fileId);

        FullUserFile GetFile(string userId, string fileId);

        bool UpdateName(string userId, string fileId, string newfileName);

        bool Delete(string userId, string fileId);
    }
}
