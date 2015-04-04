using System.Collections.Generic;
using System.Linq;
using Cloud.Repositories.Common;
using Cloud.Repositories.DataContext;
using Cloud.Repositories.Models;

namespace Cloud.Repositories.Repositories
{
    public class UserFileRepository : RepositoryBase
    {
        public bool AddFile(UserFileModel file)
        {
            // Save file on all physical servers
            var serverManager = new ServerManager();
            serverManager.SaveFile(file.Stream, file.UserFile.Name, file.UserFile.UserId);

            // Save file info to Db
            Add(file.UserFile, true);

            return true;
        }

        public UserFile GetFile(string userId, int fileId)
        {
            return Entities.UserFiles.SingleOrDefault(
                file => file.UserId == userId && file.FileId == fileId);
        }

        public IEnumerable<UserFile> GetFiles(string userId)
        {
            return Entities.UserFiles.Where(file => file.UserId == userId);
        }

        // todo: test
        // todo: implement return type info
        public bool UpdateFileName(string userId, int fileId, string fileName)
        {
            var fileToUpdate = Entities.UserFiles.SingleOrDefault(
                file => file.FileId == fileId && file.UserId == userId);
            if (fileToUpdate == null) return false;

            Entities.UserFiles.Attach(fileToUpdate);
            var entry = Entities.Entry(fileToUpdate);
            entry.Property(file => file.Name).IsModified = true;
            SaveChanges();

            return true;
        }

        public bool DeleteFile(string userId, int fileId)
        {
            var fileToDelete = Entities.UserFiles.SingleOrDefault(
                file => file.FileId == fileId && file.UserId == userId);
            if (fileToDelete == null) return false;

            // Delete file from all servers

            var serverManager = new ServerManager();
            serverManager.DeleteFile(userId, fileToDelete.Name);

            // Delete file from db
            Entities.UserFiles.Attach(fileToDelete);
            Entities.UserFiles.Remove(fileToDelete);
            SaveChanges();

            return true;
        }
    }
}