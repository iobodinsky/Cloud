using Cloud.Common.Models;

namespace Cloud.Common.Interfaces {
	public interface IStorage {
		void Authorize();

		void AddFile( string userId, FullUserFile file );

		void AddFolder( string userId, IFolder folder );

		FolderData GetRootFolderData( string userId );

		FolderData GetFolderData( string userId, string folderId );

		IFile GetFileInfo( string userId, string fileId );

		FullUserFile GetFile( string userId, string fileId );

		void UpdateName( string userId, string fileId, string newfileName );

		void DeleteFile(string userId, string fileId);

		void DeleteFolder(string userId, string folderId);
	}
}
