using restlessmedia.Module.Twitter;
using restlessmedia.Module.Web.Api.Attributes;
using System;
using System.Web.Http;

namespace restlessmedia.Module.Web.Api.Controllers
{
  public class TwitterController : ApiControllerBase
  {
    public TwitterController(IUIContext context, ITwitterService twitterService)
      : base(context)
    {
      _twitterService = twitterService ?? throw new ArgumentNullException(nameof(twitterService));
    }

    [HttpGet]
    [ClientCache(4)]
    [Route("api/twitter/{username}/latest")]
    [Cache(48)]
    public Tweet Latest(string username)
    {
      return _twitterService.Latest(username);
    }

    private readonly ITwitterService _twitterService;
  }
}