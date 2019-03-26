using System;
using System.Web.Http;
using System.Web.Http.Results;

namespace restlessmedia.Module.Web.Api.Controllers
{
  public abstract class ApiControllerBase : ApiController, IController
  {
    public ApiControllerBase(IUIContext context)
    {
      Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IHttpActionResult Error(Exception exception, bool includeDetail = false)
    {
      if (includeDetail)
      {
        return new ExceptionResult(exception, true, Configuration.Services.GetContentNegotiator(), Request, Configuration.Formatters);
      }

      return InternalServerError(exception);
    }

    public IHttpActionResult TryResult(Action fn)
    {
      try
      {
        fn();
      }
      catch (Exception e)
      {
        return Error(e);
      }

      return Ok();
    }

    public IHttpActionResult ModelStateResult()
    {
      return Content(System.Net.HttpStatusCode.BadRequest, ModelState);
    }

    public IUIContext Context { get; private set; }
  }
}