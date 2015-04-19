using System;
using Cloud.Common.Interfaces;

namespace Cloud.StoragesApi.Models
{
    class DriveFile : IFile
    {
        public int FileId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }
        public string Path { get; set; }
        public bool IsEditable { get; set; }
        public long Size { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public DateTime AddedDateTime { get; set; }
        public int DownloadedTimes { get; set; }
    }
}
