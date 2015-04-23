using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Types;
using Cloud.Repositories.Common;

namespace Cloud.Repositories.Repositories
{
    public class StorageRepository : RepositoryBase, IFileRepository
    {
        public bool Add(string userId, int cloudId, FullUserFile file)
        {
            // Save file on all physical servers
            var serverManager = new LocalFileServerManager();
            serverManager.SaveFile(file.Stream, file.UserFile.Name, file.UserFile.UserId);

            // Save file info to Db
            Add(file.UserFile, true);

            return true;
        }

        public IFile Get(string userId, int cloudId, string fileId)
        {
            return Entities.UserFiles.SingleOrDefault(
                file => file.UserId == userId && file.Id == fileId);
        }

        public IEnumerable<IFile> GetRootFiles(string userId)
        {
            return Entities.UserFiles.Where(file => file.UserId == userId);
        }

        public IEnumerable<IFolder> GetRootFolders(string userId)
        {
            throw new NotImplementedException();
            //return Entities.UserFiles.Where(file => file.UserId == userId);
        }

        // todo: test
        // todo: implement return type info
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
    }
}