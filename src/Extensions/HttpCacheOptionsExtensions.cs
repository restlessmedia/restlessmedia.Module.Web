using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Linq;
using System;

namespace restlessmedia.Module.Web
{
  public static class HttpCacheOptionsExtensions
  {
    public static void Apply(this HttpCacheOptions options, HttpCachePolicyBase policy)
    {
      if (policy == null)
      {
        return;
      }

      policy.SetCacheability(options.IsPublic ? HttpCacheability.Public : HttpCacheability.Private);

      if (options.LastModified.HasValue)
      {
        policy.SetLastModified(options.LastModified.Value);
      }

      if (!string.IsNullOrEmpty(options.ETag))
      {
        policy.SetETag(options.QuotedETag);
      }

      if (options.MaxAge.HasValue)
      {
        policy.SetMaxAge(options.MaxAge.Value);
      }
    }

    public static void Apply(this HttpCacheOptions options, HttpResponseMessage response)
    {
      if (response == null)
      {
        return;
      }

      response.Headers.CacheControl = new CacheControlHeaderValue();

      if (options.IsPublic || options.MaxAge.HasValue)
      {
        response.Headers.CacheControl.Public = true;
      }

      if (options.MaxAge.HasValue)
      {
        response.Headers.CacheControl.MaxAge = response.Headers.Age = options.MaxAge;
      }

      if (response.Content != null)
      {
        if (options.LastModified.HasValue && response.Content != null)
        {
          response.Content.Headers.LastModified = options.LastModified.Value;
        }

        response.Content.Headers.Expires = Expires(response.Headers);
      }

      if (!string.IsNullOrEmpty(options.ETag))
      {
        response.Headers.ETag = new EntityTagHeaderValue(options.QuotedETag);
      }
    }

    public static bool ETagMatches(this HttpCacheOptions options, HttpRequestBase request)
    {
      if (request == null)
      {
        return false;
      }

      const string key = "if-None-Match";
      return ETagMatches(options, request.Headers[key]);
    }

    public static bool ETagMatches(this HttpCacheOptions options, string value)
    {
      return options.QuotedETag == value;
    }

    public static bool ETagMatches(this HttpCacheOptions options, HttpRequestMessage request)
    {
      if (request == null)
      {
        return false;
      }

      return request.Headers.IfNoneMatch.Any(x => x.Tag == options.QuotedETag);
    }

    public static DateTimeOffset? Expires(this HttpResponseHeaders headers)
    {
      if (headers.CacheControl == null || !headers.CacheControl.MaxAge.HasValue)
      {
        return null;
      }

      return DateTime.Now.Add(headers.CacheControl.MaxAge.Value);
    }
  }
}