using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.ApplicationInsights;
using restlessmedia.Module.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace restlessmedia.Module.Web
{
  /// <summary>
  /// Provides the base class for mvc and web api http applications
  /// </summary>
  public abstract class HttpApplication : System.Web.HttpApplication
  {
    public virtual void Application_Start()
    {
      HttpConfiguration configuration = GlobalConfiguration.Configuration;
      ContainerBuilder builder = new ContainerBuilder();
      IEnumerable<IWebModule> webModules = ModuleLoader<IWebModule>.FindModules();
      GlobalConfiguration.Configure((config) => webModules.ForEach(webModule => webModule.Init(config, builder)));
      IContainer container =  builder.Build();

      webModules.ForEach(webModule => webModule.Init(configuration, container));

      SetDefaultCulture();

      // added for odata
      configuration.Initializer(configuration);
    }

    public virtual void Session_Start(object sender, EventArgs e)
    {
      string id = Session.SessionID;
    }

    public virtual void SetDefaultCulture(string culture = "en-GB")
    {
      System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo(culture);

      if (System.Globalization.CultureInfo.DefaultThreadCurrentUICulture != cultureInfo)
      {
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
      }

      if (System.Globalization.CultureInfo.DefaultThreadCurrentCulture != cultureInfo)
      {
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
      }
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      HttpApplication application;

      // fixes garbage output when using GZIP http://stackoverflow.com/questions/2416182/garbled-error-page-output-using-gzip-in-asp-net-iis7/4655371#4655371
      if ((application = sender as HttpApplication) != null)
      {
        // fix for error when setting filter
        // http://romsteady.blogspot.co.uk/2008/12/workaround-aspnet-response-filter-is.html
        // There is a bug in ASP.NET 3.5 where you cannot set the Response.Filter property unless you read it first. When the Response.Filter property is first read, it instantiates some internal fields in the HttpWriter class.
        try
        {
          Stream s = application.Response.Filter;
          application.Response.Filter = null;
        }
        finally
        {
           // TODO: catch specific exception when accessing response?
        }
      }

      Exception lastException = Server.GetLastError();

      if (HttpContext.Current.IsCustomErrorEnabled && lastException != null)
      {
        new TelemetryClient().TrackException(lastException);
      }
    }

    protected virtual void Application_PostAuthenticateRequest(object sender, EventArgs e)
    {
      Resolve<IWebSecurityProvider>().Authenticate(Context);
    }

    protected virtual void Application_AcquireRequestState(object sender, EventArgs e)
    {
      Resolve<IWebSecurityProvider>().Acquire(Context);
    }

    private T Resolve<T>()
    {
      return DependencyResolver.Current.GetService<T>();
    }
  }
}