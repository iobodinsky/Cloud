using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Cloud.Common.Interfaces;
using Cloud.Common.Managers;
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
                storages.Add(StorageFactory.ResolveInstance(storage.Id, storage.ClassName));
            }

            var foldersData = new List<FolderData>();
            foreach (var storage in storages)
            {
                foldersData.Add(await storage.GetRootFolderDataAsync(UserId));
            }

            return Ok(foldersData);
        }

        // GET api/folders/1/storage/1
        [Route("{folderId}/storage/{storageId:int}")]
        public async Task<IHttpActionResult> GetFolderData([FromUri] string folderId,
            [FromUri] int storageId)
        {
            var storage = StorageFactory.ResolveInstance(storageId);
            var folderDada = await storage.GetFolderDataAsync(UserId, folderId);

            return Ok(folderDada);
        }

        // POST: api/folders/storage/1/create
        [Route("storage/{storageId:int}/create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(
            [FromUri] int storageId, [FromBody] UserFolder folder)
        {
            var storage = StorageFactory.ResolveInstance(storageId);
            var folderId = new IdGenerator().ForFolder();
            folder.Id = folderId;
            folder.UserId = UserId;
            folder.StorageId = storageId;
            var createdFolder = await storage.AddFolderAsync(UserId, folder);

            return Ok(createdFolder);
        }

        // POST api/folders/1/storage/1/rename
        [Route("{folderId}/storage/{storageId:int}/rename")]
        [HttpPost]
        public async Task<IHttpActionResult> RenameFolder([FromUri] string folderId, [FromUri] int storageId,
            [FromBody] NewNameModel newFolder)
        {
            if (string.IsNullOrEmpty(newFolder.Name)) return BadRequest();
            var storage = StorageFactory.ResolveInstance(storageId);
            var newName = await storage.UpdateFolderNameAsync(UserId, folderId, newFolder.Name);

            return Ok(newName);
        }

        // DELETE: api/folders/1/storage/1/delete
        [Route("{folderId}/storage/{storageId:int}/delete")]
        public async Task<IHttpActionResult> Delete([FromUri] string folderId, [FromUri] int storageId)
        {
            var storage = StorageFactory.ResolveInstance(storageId);
            await storage.DeleteFolderAsync(UserId, folderId);

            return Ok(folderId);
        }
    }
}