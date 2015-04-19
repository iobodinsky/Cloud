using System;

namespace Cloud.Common.Interfaces
{
    public interface IFile
    {
        int FileId { get; set; }
        string UserId { get; set; }
        string Name { get; set; }
        int? TypeId { get; set; }
        string Path { get; set; }
        bool IsEditable { get; set; }
        long Size { get; set; }
        DateTime LastModifiedDateTime { get; set; }
        DateTime AddedDateTime { get; set; }
        int DownloadedTimes { get; set; }
    }
}
