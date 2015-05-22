using System.Collections.Generic;
using Cloud.Common.Models;

namespace Cloud.Common.Interfaces {
	public interface IFolderRepository {
		void Add(string userId, int cloudId, IFolder file);

		IFolder GetFolder(string userId, int cloudId, string folderId);

		IEnumerable<IFile> GetRootFiles(string userId);

		IEnumerable<IFolder> GetRootFolders(string userId);

		string GetRootFolderId(string userId);

		void UpdateName(string userId, int cloudId, string folderId, string newfolderName);

		void Delete(string userId, int cloudId, string folderId);
	}
}
