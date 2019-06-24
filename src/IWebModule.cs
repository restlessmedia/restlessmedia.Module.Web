using Autofac;
using System.Collections.Generic;
using System.Web.Http;

namespace restlessmedia.Module.Web
{
  /// <summary>
  /// Interface for reistering a web module (mvc, api etc)
  /// </summary>
  public interface IWebModule
  {
    void OnStart(HttpConfiguration httpConfiguration, ContainerBuilder builder, IEnumerable<IWebModule> webModules);

    void OnStart(HttpConfiguration httpConfiguration, IContainer container, IEnumerable<IWebModule> webModules);
    
    // called once all modules have been started
    void OnStarted(HttpConfiguration httpConfiguration, IContainer container, IEnumerable<IWebModule> webModules);
  }
}
