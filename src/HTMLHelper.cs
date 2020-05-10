using System.Text.RegularExpressions;
using System.Web;

namespace restlessmedia.Module.Web
{
  public static class HTMLHelper
  {
    /// <summary>
    /// This will parse any text for urls and wrap them with <a></a>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Anchorize(string value, string target = "_blank")
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        return value;
      }

      const string UrlMatch = "((mailto\\:|(news|(ht|f)tp(s?))\\://){1}\\S+)";
      string result = value;

      foreach (Match match in Regex.Matches(value, UrlMatch))
      {
        result = result.Replace(match.Value, $"<a href=\"{match.Value}\" target=\"{target}\">{match.Value}</a>");
      }

      return result;
    }

    /// <summary>
    /// Converts an html to text
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string ToText(string html)
    {
      if (string.IsNullOrWhiteSpace(html))
      {
        return html;
      }

      const string matchTagPattern = @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[\^'"">\s]+))?)+\s*|\s*)/?>";
      return Regex.Replace(html, matchTagPattern, string.Empty).Trim();
    }

    /// <summary>
    /// If true, the html contains at least one tag with innerTEXT.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static bool ContainsInnerText(string html)
    {
      if (string.IsNullOrWhiteSpace(html))
      {
        return false;
      }

      // first strip all the tags then check the length of the remaining string
      return ToText(html).Length > 0;
    }
  }
}