using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
{
    public class UserFileRepository : RepositoryBase
    {
        public IEnumerable<UserFile> GetAllFiles(string userId)
        {
            return Entities.UserFiles.Where(file => file.UserId == userId);
        }

        public void Delete(int fileId)
        {
            var fileToDelete = Entities.UserFiles.First(file => file.FileId == fileId);
            if (fileToDelete == null) return;

            // Delete file from storage first
            var filePath = Path.Combine(fileToDelete.Path, fileToDelete.Name);
            File.Delete(filePath);

            // Delete file from db
            Entities.UserFiles.Attach(fileToDelete);
            Entities.UserFiles.Remove(fileToDelete);
            Entities.SaveChanges();
        }
    }
}