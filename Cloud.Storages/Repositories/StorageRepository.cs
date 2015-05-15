using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Storages.DataContext;
using Cloud.Storages.Managers;
using Cloud.Storages.Providers;

namespace Cloud.Storages.Repositories
{
    public class StorageRepository : RepositoryBase, IFileRepository
    {
        #region Fields

        private readonly IList<IStorage> _storages; 

        #endregion Fields

        public StorageRepository()
        {
            // todo: implement MEF
            _storages = new List<IStorage>
            {
                new LocalLenevoProvider(), 
                new DriveProvider()
            };
        }

        #region Private methods

        private IStorage ResolveStorageInstance(int cloudId)
        {
            var cloudServer = Entities.CloudServers.
                SingleOrDefault(server => server.Id == cloudId);
            if (cloudServer == null) return null;
            var cloudType = Type.GetType(cloudServer.ClassName);
            if (cloudType == null) return null;
            var cloud = Activator.CreateInstance(cloudType) as IStorage;

            return cloud;
        }

        #endregion Private methods

        #region IFileRepository implementation

        public bool Add(string userId, int cloudId, FullUserFile file)
        {
            var cloud = ResolveStorageInstance(cloudId);
            cloud.Add(userId, file);
            
            return true;
        }

        public IFile GetFileInfo(string userId, int cloudId, string fileId)
        {
            throw new NotImplementedException();
        }

        public FullUserFile GetFile(string userId, int cloudId, string fileId)
        {
            var cloud = ResolveStorageInstance(cloudId);
            return cloud.GetFile(userId, fileId);
        }

        public IFile Get(string userId, int cloudId, string fileId)
        {
            return Entities.UserFiles.SingleOrDefault(
                file => file.UserId == userId && file.Id == fileId);
        }

        public IEnumerable<IFile> GetRootFiles(string userId)
        {
            var files = new List<IFile>();
            foreach (var storage in _storages)
            {
                files.AddRange(storage.GetRootFiles(userId));
            }

            return files;
        }

        public IEnumerable<IFolder> GetRootFolders(string userId)
        {
            var folders = new List<IFolder>();
            foreach (var storage in _storages)
            {
                folders.AddRange(storage.GetRootFolders(userId));
            }

            return folders;
        }

        public bool UpdateName(string userId, int cloudId, string fileId, string newfileName)
        {
            var fileToUpdate = Entities.UserFiles.SingleOrDefault(
                file => file.Id == fileId && file.UserId == userId);
            if (fileToUpdate == null) return false;

            // Rename file on servers
            var oldfileName = fileToUpdate.Name;
            var extention = Path.GetExtension(oldfileName);
            newfileName += extention;
            var serverManager = new LocalFileServerManager();
            if (!serverManager.RenameFile(userId, fileId, oldfileName, newfileName))
                return false;

            // Rename file in Db
            fileToUpdate.Name = newfileName;
            Entities.UserFiles.Attach(fileToUpdate);
            var entry = Entities.Entry(fileToUpdate);
            entry.Property(file => file.Name).IsModified = true;
            SaveChanges();

            return true;
        }

        public bool Delete(string userId, int cloudId, string fileId)
        {
            var fileToDelete = Entities.UserFiles.SingleOrDefault(
                file => file.Id == fileId && file.UserId == userId);
            if (fileToDelete == null) return false;

            // Delete file from all servers

            var serverManager = new LocalFileServerManager();
            serverManager.DeleteFile(userId, fileToDelete.Name);

            // Delete file from db
            Entities.UserFiles.Attach(fileToDelete);
            Entities.UserFiles.Remove(fileToDelete);
            SaveChanges();

            return true;
        }

        #endregion IFileRepository implementation
    }
}