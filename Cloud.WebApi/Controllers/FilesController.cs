using System.Threading.Tasks;
using System.Web.Http;
using Cloud.WebApi.Models;

namespace Cloud.WebApi.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiControllerBase
    {
        // GET api/files/1/download/dropbox
        [Route("{fileId}/download/dropbox")]
        public async Task<IHttpActionResult> GetDownloadUrlDropbox([FromUri] string fileId)
        {
            var downloadUrl = await StorageFactory.GetDropboxInstance()
                .GetDownloadUrl(UserId, fileId);

            return Ok(downloadUrl);
        }

        //// POST api/files/storage/1/folder/1/upload
        //[Route("storage/{storageAlias}/folder/{folderId}/upload")]
        //[HttpPost]
        //public async Task<IHttpActionResult> UploadFile( [FromUri] string storageAlias,
        //    [FromUri] string folderId ) {

        //    var httpRequest = HttpContext.Current.Request;

        //    var postedFile = httpRequest.Files.Get(0);
        //    var file = new UserFile {
        //        Id = new IdGenerator().ForFile(),
        //        Name = postedFile.FileName,
        //        AddedDateTime = DateTime.Now,
        //        IsEditable = true,
        //        UserId = UserId,
        //        Size = postedFile.ContentLength,
        //        DownloadedTimes = 0,
        //        LastModifiedDateTime = DateTime.Now,
        //        FolderId = folderId,
        //        StorageId = storageId
        //    };

        //    var userFileModel = new FullUserFile {
        //        UserFile = file,
        //        Stream = postedFile.InputStream
        //    };

        //    var storage = GetStorageInstance(storageId);
        //    var createdFile = await storage.AddFileAsync(
        //        UserId, userFileModel);

        //    return Ok(createdFile);
        //}

        // POST api/files/1/storage/1/rename
        [Route("{fileId}/storage/{storageAlias}/rename")]
        [HttpPost]
        public async Task<IHttpActionResult> RenameFile([FromUri] string fileId,
            [FromUri] string storageAlias, [FromBody] NewNameModel newfile)
        {
            if (string.IsNullOrEmpty(newfile.Name))
            {
                // todo:
                return BadRequest();
            }
            var storage = StorageFactory.ResolveInstance(storageAlias);
            var newFileName = await storage.UpdateFileNameAsync(UserId, fileId, newfile.Name);

            return Ok(newFileName);
        }

        // DELETE api/files/1/storage/1/delete
        [Route("{fileId}/storage/{storageAlias:min(1)}/delete")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteFile([FromUri] string fileId,
            [FromUri] string storageAlias)
        {
            var storage = StorageFactory.ResolveInstance(storageAlias);
            await storage.DeleteFileAsync(UserId, fileId);

            return Ok();
        }
    }
}