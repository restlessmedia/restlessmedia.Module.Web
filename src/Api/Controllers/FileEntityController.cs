using restlessmedia.Module.File;
using restlessmedia.Module.Web.Api.Extensions;
using restlessmedia.Module.Web.Api.Upload;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace restlessmedia.Module.Web.Api.Controllers
{
  public class FileEntityController : ApiControllerBase
  {
    public FileEntityController(IUIContext context, IFileService fileService, IDiskStorageProvider storageProvider)
      : base(context)
    {
      _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
      _storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
    }

    [HttpGet]
    [Route("api/file/{entityType}/{entityId}")]
    public IEnumerable<FileEntity> Get(EntityType entityType, int entityId)
    {
      return _fileService.List(entityType, entityId);
    }

    [HttpPost]
    [Route("api/file/{entityType}/{entityId}/{path}")]
    public Task<IHttpActionResult> Post([FromUri]EntityType entityType, [FromUri]int entityId, [FromUri]string path)
    {
      IUploadHandler uploadHandler = new FileUploadHandler(_fileService, _storageProvider, entityType, entityId, path);
      return Request.UploadAsync(uploadHandler.GetStream, uploadHandler.Done);
    }

    [HttpDelete]
    [Route("api/file/{fileId}")]
    public IHttpActionResult Delete(string path, int fileId)
    {
      return TryResult(() => _fileService.Delete(path, fileId));
    }

    private readonly IFileService _fileService;

    private readonly IDiskStorageProvider _storageProvider;
  }
}