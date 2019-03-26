using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace restlessmedia.Module.Web.Api.Attributes
{
  public class BasicAuthenticationAttribute : AuthorizeAttribute
  {
    public BasicAuthenticationAttribute(string username, string password, bool isBase64 = true)
    {
      if (string.IsNullOrEmpty(username))
      {
        throw new ArgumentNullException(nameof(username));
      }

      if (string.IsNullOrEmpty(password))
      {
        throw new ArgumentNullException(nameof(password));
      }

      _username = username;
      _password = password;
      _isBase64 = isBase64;
    }

    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
      return actionContext.Request.Headers.Authorization.IsAuthorized(_username, _password, _isBase64);
    }

    protected IUIContext Context
    {
      get
      {
        if (_context == null)
        {
          _context = Resolve<IUIContext>();
        }

        return _context;
      }
    }

    protected T Resolve<T>()
    {
      return System.Web.Mvc.DependencyResolverExtensions.GetService<T>(System.Web.Mvc.DependencyResolver.Current);
    }

    private IUIContext _context;

    private string _username;

    private string _password;

    private bool _isBase64 = true;
  }
}
