using System;
using System.Collections.Generic;
using Cloud.Common.Interfaces;
using Cloud.Repositories.Models;

namespace Cloud.Repositories.Repositories.ExternalStorages
{
    public class DriveRepository : IFileRepository
    {
        public bool AddFile(FullUserFile file)
        {
            throw new NotImplementedException();
        }

        public IFile GetFile(string userId, int fileId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFile> GetFiles(string userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateFileName(string userId, int fileId, string newfileName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(string userId, int fileId)
        {
            throw new NotImplementedException();
        }
    }
}
