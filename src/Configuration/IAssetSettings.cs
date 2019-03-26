using System.Collections.Generic;

namespace restlessmedia.Module.Web.Configuration
{
  public interface IAssetSettings
  {
    string Version { get; }

    IEnumerable<IInclude> Includes { get; }

    string ResolvePath(string path);
  }
}