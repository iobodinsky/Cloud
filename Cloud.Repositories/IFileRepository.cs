using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Repositories.Models;

namespace Cloud.Repositories
{
    public interface IFileRepository
    {
        bool AddFile(FullUserFile file);

        IFile GetFile(string userId, int fileId);

        IEnumerable<IFile> GetFiles(string userId);

        bool UpdateFileName(string userId, int fileId, string newfileName);

        bool DeleteFile(string userId, int fileId);
    }
}
