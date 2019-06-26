using Autofac;
using System.Collections.Generic;
using System.Web.Http;

namespace restlessmedia.Module.Web
{
  /// <summary>
  /// Base web module class implementation of <see cref="IWebModule" />.
  /// </summary>
  public abstract class WebModuleBase : IWebModule
  {
    public virtual void OnStart(HttpConfiguration httpConfiguration, ContainerBuilder builder, IEnumerable<IWebModule> webModules) { }

    public virtual void OnStart(HttpConfiguration httpConfiguration, IContainer container, IEnumerable<IWebModule> webModules) { }

    public virtual void OnStarted(HttpConfiguration httpConfiguration, IContainer container, IEnumerable<IWebModule> webModules) { }
  }
}