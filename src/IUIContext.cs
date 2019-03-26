using restlessmedia.Module.Configuration;
using restlessmedia.Module.Email.Configuration;
using restlessmedia.Module.File;
using restlessmedia.Module.File.Configuration;
using restlessmedia.Module.Security;
using restlessmedia.Module.Web.Configuration;

namespace restlessmedia.Module.Web
{
  public interface IUIContext
  {
    IAssetSettings AssetSettings { get; }

    IUserInfo AuthenticatedUser { get; }

    bool IsAuthenticated { get; }

    ISecurityService Security { get; }

    IFileService File { get; }

    ICacheProvider Cache { get; }

    IFileSettings FileSettings { get; }

    IEmailSettings EmailSettings { get; }

    ILicenseSettings LicenseSettings { get; }
  }
}