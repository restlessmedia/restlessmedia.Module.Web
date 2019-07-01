using Autofac;
using restlessmedia.Module.Configuration;
using restlessmedia.Module.Email.Configuration;
using restlessmedia.Module.File;
using restlessmedia.Module.File.Configuration;
using restlessmedia.Module.Security;
using restlessmedia.Module.Web.Configuration;

namespace restlessmedia.Module.Web
{
  public class Module : IModule
  {
    public void RegisterComponents(ContainerBuilder containerBuilder)
    {
      containerBuilder.RegisterType<UIContext>().As<IUIContext>().SingleInstance();
      containerBuilder.RegisterSettings<IAssetSettings>("restlessmedia/asset", required: false);

      containerBuilder.RegisterWhen<IUIContext>(
        x => x.IsRegistered<IAssetSettings>(),
        x => new UIContext(x.Resolve<ISecurityService>(), x.Resolve<IFileService>(), x.Resolve<ICacheProvider>(), x.Resolve<IAssetSettings>(), x.Resolve<IFileSettings>(), x.Resolve<IEmailSettings>(), x.Resolve<ILicenseSettings>()),
        x => new UIContext(x.Resolve<ISecurityService>(), x.Resolve<IFileService>(), x.Resolve<ICacheProvider>(), x.Resolve<IFileSettings>(), x.Resolve<IEmailSettings>(), x.Resolve<ILicenseSettings>())
        );
    }
  }
}