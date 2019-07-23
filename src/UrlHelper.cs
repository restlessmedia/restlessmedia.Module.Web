using FastMember;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace restlessmedia.Module.Web
{
  public class UrlHelper
  {
    /// <summary>
    /// Returns a friendly url
    /// </summary>
    /// <param name="urlToEncode"></param>
    /// <returns></returns>
    public static string ToFriendlyUrl(string urlToEncode)
    {
      urlToEncode = (urlToEncode ?? string.Empty).Trim().ToLower();

      // trim excess whitespace
      if (!string.IsNullOrEmpty(urlToEncode))
      {
        urlToEncode = Regex.Replace(urlToEncode, @"\s+", " ");
      }

      StringBuilder url = new StringBuilder();

      foreach (char ch in urlToEncode)
      {
        switch (ch)
        {
          case ' ':
            url.Append('-');
            break;
          case '&':
            url.Append("and");
            break;
          case '\'':
            break;
          default:
            if ((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'z'))
            {
              url.Append(ch);
            }
            break;
        }
      }

      return url.ToString();
    }

    /// <summary>
    /// Converts the object into member key/value pairs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ConvertToQueryString<T>(T obj)
    {
      ObjectAccessor objectAccessor = ObjectAccessor.Create(obj);
      string[] pairs = TypeAccessor.Create(typeof(T)).GetMembers().Where(x => !x.IsDefined(typeof(IgnoreAttribute))).Select(x => string.Concat(x.Name, "=", objectAccessor[x.Name])).ToArray();
      return string.Concat("?", string.Join("&", pairs));
    }
  }
}