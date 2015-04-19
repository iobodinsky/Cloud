using System.Collections.Generic;
using Cloud.Common.Types;

namespace Cloud.Common.Interfaces
{
    public interface IStorage
    {
        bool Add(string userId, FullUserFile file);

        IEnumerable<IFile> GetRootFiles(string userId);

        IEnumerable<IFile> GetFilesIn(string userId,string folder);
            
        IEnumerable<IFolder> GetRootFolders(string userId);

        IEnumerable<IFolder> GetFoldersIn(string userId, string folder);

        IFile Get(string userId, int fileId);

        bool UpdateName(string userId, int fileId, string newfileName);

        bool Delete(string userId, int fileId);
    }
}
