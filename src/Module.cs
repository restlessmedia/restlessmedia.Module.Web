using Autofac;
using restlessmedia.Module.Web.Configuration;

namespace restlessmedia.Module.Web
{
  public class Module : IModule
  {
    public void RegisterComponents(ContainerBuilder containerBuilder)
    {
      containerBuilder.RegisterSettings<IAssetSettings>("restlessmedia/asset", required: false);
    }
  }
}