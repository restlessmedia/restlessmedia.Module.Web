using Autofac;
using Microsoft.ApplicationInsights;
using restlessmedia.Module.Configuration;
using restlessmedia.Module.Data;
using restlessmedia.Module.Security;
using SqlBuilder.DataServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

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
      ContainerBuilder containerBuilder = new ContainerBuilder();
      IEnumerable<IWebModule> webModules = ModuleLoader<IWebModule>.FindModules();

      // register all modules
      ModuleBuilder.RegisterModules(containerBuilder);

      // register web modules with containerbuilder
      GlobalConfiguration.Configure((config) => webModules.ForEach(webModule => webModule.OnStart(config, containerBuilder, webModules)));

      // build
      _container =  containerBuilder.Build();

      // web modules OnStart
      webModules.ForEach(webModule => webModule.OnStart(configuration, _container, webModules));

      SetDefaultCulture();

      // added for odata
      configuration.Initializer(configuration);

      // web modules OnStarted
      webModules.ForEach(webModule => webModule.OnStarted(configuration, _container, webModules));

      // subscribe to the oncreating event for model data service to set context on the connection
      // this is only performed for web modules
      ModelDataService.OnCreate += (sender, connection) =>
      {
        // set context
        LicenseHelper.SetContext(connection, _container.Resolve<ILicenseSettings>());
      };
    }

    public virtual void Session_Start(object sender, EventArgs e)
    {
      // TODO: work out why we've done this - is it to initialise the session?!
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

    /// <summary>
    /// Fired when the web application ends
    /// </summary>
    protected virtual void Application_End()
    {
      _container.Dispose();
      ModelDataService.ClearAllEvents();
    }

    private T Resolve<T>()
    {
      return _container.Resolve<T>();
    }

    private static IContainer _container;
  }
}
