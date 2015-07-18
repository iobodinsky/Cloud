using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.WebApi.Models;

namespace Cloud.WebApi.Controllers
{
    [RoutePrefix("api/folders")]
    public class FoldersController : ApiControllerBase
    {
        // GET api/folders
        [Route("")]
        public async Task<IHttpActionResult> GetRootFolderData()
        {
            var connectedStorages = UserStoragesRepository.GetStorages(UserId);
            var storages = new List<IStorage>();

            foreach (var storage in connectedStorages)
            {
                storages.Add(StorageFactory.ResolveInstance(storage.Alias));
            }

            var foldersData = new List<FolderData>();
            foreach (var storage in storages)
            {
                foldersData.Add(await storage.GetRootFolderDataAsync(UserId));
            }

            return Ok(foldersData);
        }

        // GET api/folders/1/storages/1
        [Route("{folderId}/storages/{storageAlias}")]
        public async Task<IHttpActionResult> GetFolderData([FromUri] string folderId,
            [FromUri] string storageAlias)
        {
            var storage = StorageFactory.ResolveInstance(storageAlias);
            var folderDada = await storage.GetFolderDataAsync(UserId, folderId);

            return Ok(folderDada);
        }

        // POST: api/folders/storages/1/create
        [Route("storages/{storageAlias}/create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(
            [FromUri] string storageAlias, [FromBody] UserFolder folder)
        {
            var storage = StorageFactory.ResolveInstance(storageAlias);
            folder.Storage = storageAlias;
            folder.UserId = UserId;

            var createdFolder = await storage.AddFolderAsync(UserId, folder);

            return Ok(createdFolder);
        }

        // POST api/folders/1/storages/1/rename
        [Route("{folderId}/storages/{storageAlias}/rename")]
        [HttpPost]
        public async Task<IHttpActionResult> RenameFolder([FromUri] string folderId, [FromUri] string storageAlias,
            [FromBody] NewNameModel newFolder)
        {
            if (string.IsNullOrEmpty(newFolder.Name)) return BadRequest();
            var storage = StorageFactory.ResolveInstance(storageAlias);
            var newName = await storage.UpdateFolderNameAsync(UserId, folderId, newFolder.Name);

            return Ok(newName);
        }

        // DELETE: api/folders/1/storages/1/delete
        [Route("{folderId}/storages/{storageAlias}/delete")]
        public async Task<IHttpActionResult> Delete([FromUri] string folderId, [FromUri] string storageAlias)
        {
            var storage = StorageFactory.ResolveInstance(storageAlias);
            await storage.DeleteFolderAsync(UserId, folderId);

            return Ok(folderId);
        }
    }
}