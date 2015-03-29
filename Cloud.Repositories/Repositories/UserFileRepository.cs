using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
{
    public class UserFileRepository : RepositoryBase
    {
        public void AddFile(UserFile file)
        {
            Add(file, true);
        }

        public UserFile GetFile(string userId, int fileId)
        {
            return  Entities.UserFiles.SingleOrDefault(
                file => file.UserId == userId && file.FileId == fileId);
        }

        public IEnumerable<UserFile> GetFiles(string userId)
        {
            return Entities.UserFiles.Where(file => file.UserId == userId);
        }

        public bool UpdateFileName(int fileId)
        {
            var fileToUpdate = Entities.UserFiles.SingleOrDefault(
                file => file.FileId == fileId);

            if (fileToUpdate == null) return false;

            Entities.UserFiles.Attach(fileToUpdate);
            var entry = Entities.Entry(fileToUpdate);
            entry.Property(e => e.Name).IsModified = true;
            Entities.SaveChanges();

            return true;
        }
        
        public bool DeleteFile(int fileId)
        {
            var fileToDelete = Entities.UserFiles.First(file => file.FileId == fileId);
            if (fileToDelete == null) return false;

            // Delete file from storage first
            var filePath = Path.Combine(fileToDelete.Path, fileToDelete.Name);
            File.Delete(filePath);

            // Delete file from db
            Entities.UserFiles.Attach(fileToDelete);
            Entities.UserFiles.Remove(fileToDelete);
            Entities.SaveChanges();

            return true;
        }
    }
}