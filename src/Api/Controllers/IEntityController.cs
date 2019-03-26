using System;
using System.Web.Http;

namespace restlessmedia.Module.Web.Api.Controllers
{
  [RoutePrefix("api/entity/{source}/{sourceId}")]
  public class IEntityController : ApiControllerBase
  {
    public IEntityController(IUIContext context, IEntityService entityService)
      : base(context)
    {
      _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
    }

    [HttpPost]
    [Route("move/{target}/{targetId}/{direction}")]
    public IHttpActionResult Move(EntityType source, int sourceId, EntityType target, int targetId, MoveDirection direction)
    {
      return TryResult(() => _entityService.Move(source, target, sourceId, targetId, direction));
    }

    [HttpPost]
    [Route("move/{targetId}/{direction}")]
    public IHttpActionResult Move(EntityType source, int sourceId, int targetId, MoveDirection direction)
    {
      return Move(source, sourceId, source, targetId, direction);
    }

    private readonly IEntityService _entityService;
  }
}
