using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Common.Types;

namespace Cloud.Storages.Providers
{
    internal class LocalLenevoProvider : IStorage
    {
        #region IStorage implementation

        public bool Add(string userId, FullUserFile file)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IFile> GetRootFiles(string userId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IFile> GetFilesIn(string userId, string folder)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IFolder> GetRootFolders(string userId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IFolder> GetFoldersIn(string userId, string folder)
        {
            throw new System.NotImplementedException();
        }

        public IFile Get(string userId, int fileId)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateName(string userId, int fileId, string newfileName)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(string userId, int fileId)
        {
            throw new System.NotImplementedException();
        }

        #endregion IStorage implementation
    }
}
