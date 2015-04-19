using System.Collections.Generic;
using Cloud.Common.Types;

namespace Cloud.Common.Interfaces
{
    public interface IStorage : IFileRepository
    {
        IEnumerable<IFile> GetRootFiles();

        IEnumerable<IFile> GetFilesIn(string folder);

        IEnumerable<Folder> GetRootFolders();

        IEnumerable<Folder> GetFoldersIn(string folder);
    }
}
