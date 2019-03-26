namespace restlessmedia.Module.Web.Configuration
{
  public interface IInclude
  {
    string Src { get; }

    string Alias { get; }

    string Version { get; }
  }
}