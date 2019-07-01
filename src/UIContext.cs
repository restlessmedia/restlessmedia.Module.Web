using restlessmedia.Module.Configuration;
using restlessmedia.Module.Email.Configuration;
using restlessmedia.Module.File;
using restlessmedia.Module.File.Configuration;
using restlessmedia.Module.Security;
using restlessmedia.Module.Web.Configuration;
using System;
using System.Web;

namespace restlessmedia.Module.Web
{
  public class UIContext : IUIContext
  {
    public UIContext(ISecurityService securityService, IFileService fileService, ICacheProvider cache, IFileSettings fileSettings, IEmailSettings emailSettings, ILicenseSettings licenseSettings)
    {
      Security = securityService ?? throw new ArgumentNullException(nameof(securityService));
      File = fileService ?? throw new ArgumentNullException(nameof(fileService));
      Cache = cache ?? throw new ArgumentNullException(nameof(cache));
      FileSettings = fileSettings ?? throw new ArgumentNullException(nameof(fileSettings));
      EmailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
      LicenseSettings = licenseSettings ?? throw new ArgumentNullException(nameof(licenseSettings));
    }

    public UIContext(ISecurityService securityService, IFileService fileService, ICacheProvider cache, IAssetSettings assetSettings, IFileSettings fileSettings, IEmailSettings emailSettings, ILicenseSettings licenseSettings)
      : this(securityService, fileService, cache, fileSettings, emailSettings, licenseSettings)
    {
      AssetSettings = assetSettings ?? throw new ArgumentNullException(nameof(assetSettings));
    }

    public IUserInfo AuthenticatedUser
    {
      get
      {
        if (!IsAuthenticated)
        {
          return null;
        }

        return HttpContext.Current.User as IUserInfo;
      }
    }

    /// <summary>
    /// If true, the current request contains an authenticated user
    /// </summary>
    public bool IsAuthenticated
    {
      get
      {
        HttpContext context = HttpContext.Current;

        if (context == null)
        {
          return false;
        }

        return context.User.Identity.IsAuthenticated;
      }
    }

    public ISecurityService Security { get; private set; }

    public IFileService File { get; private set; }

    public ICacheProvider Cache { get; private set; }

    public IAssetSettings AssetSettings { get; private set; }

    public IFileSettings FileSettings { get; private set; }

    public IEmailSettings EmailSettings { get; private set; }

    public ILicenseSettings LicenseSettings { get; private set; }
  }
}