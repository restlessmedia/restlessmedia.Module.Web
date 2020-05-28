using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace System
{
  public static class UriExtensions
  {
    public static string FileNameNoExtension(this Uri uri)
    {
      if (uri == null)
      {
        return null;
      }

      return Path.GetFileNameWithoutExtension(uri.LocalPath);
    }

    public static string FileName(this Uri uri)
    {
      if (uri == null)
      {
        return null;
      }

      return Path.GetFileName(uri.LocalPath);
    }

    public static string Extension(this Uri uri)
    {
      if (uri == null)
      {
        return null;
      }

      return Path.GetExtension(uri.ToString());
    }

    public static string Extension(this string url)
    {
      if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
      {
        return null;
      }

      return Extension(uri);
    }

    public static Uri AddParam(this Uri uri, string name, string value)
    {
      UriBuilder uriBuilder = new UriBuilder(uri);
      NameValueCollection paramValues = HttpUtility.ParseQueryString(uriBuilder.Query);
      paramValues[name] = value;
      uriBuilder.Query = paramValues.ToString();
      return uriBuilder.Uri;
    }
  }
}