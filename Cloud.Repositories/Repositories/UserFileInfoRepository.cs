using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
{
    public class UserFileInfoRepository : RepositoryBase
    {
        public void AddFile(UserFileInfo file)
        {
            Add(file, true);
        }

        public UserFileInfo GetFile(string userId, int fileId)
        {
            return  Entities.UserFileInfos.SingleOrDefault(
                file => file.UserId == userId && file.FileId == fileId);
        }

        public IEnumerable<UserFileInfo> GetFiles(string userId)
        {
            return Entities.UserFileInfos.Where(file => file.UserId == userId);
        }

        public bool UpdateFileName(UserFileInfo fileToUpdate)
        {
            Entities.UserFileInfos.Attach(fileToUpdate);
            var entry = Entities.Entry(fileToUpdate);
            entry.Property(file => file.Name).IsModified = true;
            SaveChanges();

            return true;
        }
        
        public bool DeleteFile(int fileId)
        {
            var fileToDelete = Entities.UserFileInfos.First(file => file.FileId == fileId);
            if (fileToDelete == null) return false;

            // Delete file from storage first
            var filePath = Path.Combine(fileToDelete.Path, fileToDelete.Name);
            File.Delete(filePath);

            // Delete file from db
            Entities.UserFileInfos.Attach(fileToDelete);
            Entities.UserFileInfos.Remove(fileToDelete);
            SaveChanges();

            return true;
        }
    }
}