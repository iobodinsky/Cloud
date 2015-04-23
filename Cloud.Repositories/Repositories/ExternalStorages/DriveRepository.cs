using System;
using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Common.Types;
using Cloud.Storages.Providers;

namespace Cloud.Repositories.Repositories.ExternalStorages
{
    public class DriveRepository : IFileRepository
    {
        #region Private fields

        private readonly IStorage _provider;

        #endregion Private fields

        public DriveRepository()
        {
            // todo: Dependency injection
            _provider = new DriveProvider();
        }

        #region IFileRepository implementation

        public bool Add(string userId, FullUserFile file)
        {
            throw new NotImplementedException();
        }

        public IFile Get(string userId, int fileId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFile> GetRootFiles(string userId)
        {
            return _provider.GetRootFiles(userId);
        }

        public IEnumerable<IFolder> GetRootFolders(string userId)
        {
            return _provider.GetRootFolders(userId);
        }

        public bool UpdateName(string userId, int fileId, string newfileName)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string userId, int fileId)
        {
            throw new NotImplementedException();
        }

        #endregion IFileRepository implementation
    }
}
