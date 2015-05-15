using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;

namespace Cloud.Storages.Providers
{
    internal class LocalLenevoProvider : IStorage
    {
        #region Private fields

        private readonly LocalFileServerManager _localFileServerManager;

        #endregion Private fields

        public LocalLenevoProvider()
        {
            _localFileServerManager = new LocalFileServerManager();
        }

        #region IStorage implementation

        public bool Add(string userId, FullUserFile file)
        {
            // Save file on all physical servers
            var serverManager = new LocalFileServerManager();
            var isSavedSuccess = serverManager.SaveFile(file.Stream, file.UserFile.Name, file.UserFile.UserId);

            // Save file info to Db
            if (!isSavedSuccess) return false;
            _localFileServerManager.Add(file.UserFile as UserFile, true);
            
            return true;
        }

        public IEnumerable<IFile> GetRootFiles(string userId)
        {
            return _localFileServerManager.GetRootFiles(userId);
        }

        public IEnumerable<IFolder> GetRootFolders(string userId)
        {
            return _localFileServerManager.GetRootFolders(userId);
        }

        public IEnumerable<IFile> GetFilesIn(string userId, string folder)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IFolder> GetFoldersIn(string userId, string folder)
        {
            throw new System.NotImplementedException();
        }

        public IFile GetFileInfo(string userId, string fileId)
        {
            throw new System.NotImplementedException();
        }

        public FullUserFile GetFile(string userId, string fileId)
        {
            return _localFileServerManager.GetFile(userId, fileId);
        }

        public bool UpdateName(string userId, string fileId, string newfileName)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(string userId, string fileId)
        {
            throw new System.NotImplementedException();
        }

        #endregion IStorage implementation
    }
}
