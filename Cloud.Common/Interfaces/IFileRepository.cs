using System.Collections.Generic;
using Cloud.Common.Models;

namespace Cloud.Common.Interfaces
{
    public interface IFileRepository
    {
        void Add(string userId, int cloudId, FullUserFile file);

        IFile GetFileInfo(string userId, int cloudId, string fileId);

        FullUserFile GetFile(string userId, int cloudId, string fileId);

        IEnumerable<IFile> GetRootFiles(string userId);

        IEnumerable<IFolder> GetRootFolders(string userId);

		  string GetRootFolderId(string userId);

		  void UpdateName(string userId, int cloudId, string fileId, string newfileName);

		  void Delete(string userId, int cloudId, string fileId);
    }
}
