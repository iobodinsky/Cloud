using System.Collections.Generic;
using Cloud.Common.Models;

namespace Cloud.Common.Interfaces
{
    public interface IStorage
    {
		  void AddFile(string userId, FullUserFile file);

		  void AddFolder(string userId, IFolder file);

        IEnumerable<IFile> GetRootFiles(string userId);

        IEnumerable<IFile> GetFilesIn(string userId,string folder);
            
        IEnumerable<IFolder> GetRootFolders(string userId);

        IEnumerable<IFolder> GetFoldersIn(string userId, string folder);

        IFile GetFileInfo(string userId, string fileId);

        FullUserFile GetFile(string userId, string fileId);

        void UpdateName(string userId, string fileId, string newfileName);

        void Delete(string userId, string fileId);
    }
}
