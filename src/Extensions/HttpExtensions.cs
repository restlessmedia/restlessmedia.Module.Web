using restlessmedia.Module.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace restlessmedia.Module.Web.Extensions
{
  public static class HttpExtensions
  {
    public static bool IsAuthorized(this HttpRequestBase httpRequest, string username, string password, bool convertBase64 = true)
    {
      return TryGetAuthHeader(httpRequest, out string authenticationUsername, out string authenticationPassword, convertBase64) && string.Compare(authenticationUsername, username) == 0 && string.Compare(authenticationPassword, password) == 0;
    }

    public static void ParseAuthCredentials(this string value, out string username, out string password, bool convertBase64 = true)
    {
      if (!string.IsNullOrEmpty(value))
      {
        if (convertBase64)
        {
          value = value.FromBase64();
        }

        if (!string.IsNullOrEmpty(value))
        {
          string[] credentials = value.Split(_basicAuthorizationDelimiter.ToCharArray());
          username = credentials.Length > 0 ? credentials[0] : null;
          password = credentials.Length > 1 ? credentials[1] : null;
          return;
        }
      }

      username = null;
      password = null;
    }

    /// <summary>
    /// Attempts to get and return a request header value with the specified key
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryGetHeader(this HttpRequestBase httpRequest, string name, out string value)
    {
      value = null;

      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentNullException(nameof(name), "Header key name cannot be null");
      }

      value = httpRequest.Headers[name];
      return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Attempts to return an "Authorization" header
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryGetAuthHeader(this HttpRequestBase httpRequest, out string value)
    {
      return TryGetHeader(httpRequest, _authorizationKey, out value);
    }

    /// <summary>
    /// Attempts to return the authorization value from an authorization header
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <param name="value"></param>
    /// <param name="convertBase64"></param>
    /// <returns></returns>
    public static bool TryGetAuthHeaderValue(this HttpRequestBase httpRequest, out string value, bool convertBase64 = true)
    {
      bool success = httpRequest.TryGetAuthHeader(out value);

      if (success)
      {
        if (value.StartsWith(_basicAuthorizationPrefix, StringComparison.OrdinalIgnoreCase))
        {
          value = value.Replace(_basicAuthorizationPrefix, null);
        }

        if (convertBase64)
        {
          value = value.FromBase64();
        }
      }

      return success;
    }

    /// <summary>
    /// Attempts to return the username and password from an "Authorization" header
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="convertBase64"></param>
    /// <returns></returns>
    public static bool TryGetAuthHeader(this HttpRequestBase httpRequest, out string username, out string password, bool convertBase64 = true)
    {
      if (TryGetAuthHeaderValue(httpRequest, out string header, false) && header.Contains(_basicAuthorizationDelimiter))
      {
        ParseAuthCredentials(header, out username, out password, convertBase64);
        return true;
      }
      else
      {
        username = null;
        password = null;
        return false;
      }
    }

    /// <summary>
    /// Converts a dictionary to a webheadercollection
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static WebHeaderCollection ToHeaderCollection(this IDictionary<string, string> values)
    {
      WebHeaderCollection headers = new WebHeaderCollection();
      
      if (values.Count > 0)
      {
        foreach (KeyValuePair<string, string> keyValue in values)
        {
          headers[keyValue.Key] = keyValue.Value;
        }
      }

      return headers;
    }

    public static string ToString(this HttpRequestBase httpRequest)
    {
      return httpRequest.ToString();
    }

    /// <summary>
    /// Wrapper for HttpContext to get the base
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static HttpContextBase GetWrapper(this HttpContext httpContext)
    {
      return new HttpContextWrapper(httpContext);
    }

    /// <summary>
    /// Returns the base uri of the site
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static Uri GetBaseUri(this HttpContextBase httpContext)
    {
      return new Uri(httpContext.Request.Url.GetLeftPart(UriPartial.Authority), UriKind.Absolute);
    }

    private const string _basicAuthorizationPrefix = "Basic ";

    private const string _basicAuthorizationDelimiter = ":";

    private const string _authorizationKey = "Authorization";
  }
}