using restlessmedia.Module.Security;
using restlessmedia.Module.Web.Api.Attributes;
using System;
using System.Web.Http;

namespace restlessmedia.Module.Web.Api.Controllers
{
  public class UserController : ApiControllerBase
  {
    public UserController(IUIContext context, ISecurityService securityService)
      : base(context)
    {
      _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
    }

    [Admin]
    [HttpGet]
    [Route("api/user/{username}/unlock")]
    public IHttpActionResult Unlock(string username)
    {
      return TryResult(() => _securityService.Unlock(username));
    }

    private readonly ISecurityService _securityService;
  }
}