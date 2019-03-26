using Autofac;
using System.Web.Http;

namespace restlessmedia.Module.Web
{
  /// <summary>
  /// Interface for reistering a web module (mvc, api etc)
  /// </summary>
  public interface IWebModule
  {
    void Init(HttpConfiguration httpConfiguration, ContainerBuilder builder);

    void Init(HttpConfiguration httpConfiguration, IContainer container);
  }
}