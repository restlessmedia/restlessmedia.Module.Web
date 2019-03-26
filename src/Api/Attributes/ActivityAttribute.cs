using restlessmedia.Module.Security;
using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace restlessmedia.Module.Web.Api.Attributes
{
  public class ActivityAttribute : AuthorizeAttribute
	{
		public ActivityAttribute(string activity, ActivityAccess access = ActivityAccess.Basic)
		{
      if (string.IsNullOrWhiteSpace(activity))
      {
        throw new ArgumentNullException(nameof(activity));
      }

      _activity = activity;
      _access = access;
		}

    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
      IUserInfo user = actionContext.RequestContext.Principal as IUserInfo;
      return user != null && SecurityService.Authorize(user, _activity, _access);
    }

    private ISecurityService SecurityService
    {
      get
      {
        if (_securityService == null)
        {
          _securityService = Resolve<ISecurityService>();
        }

        return _securityService;
      }
    }

    private T Resolve<T>()
    {
      return System.Web.Mvc.DependencyResolverExtensions.GetService<T>(System.Web.Mvc.DependencyResolver.Current);
    }

    private ISecurityService _securityService;

    private readonly string _activity;

    private readonly ActivityAccess _access;
	}
}