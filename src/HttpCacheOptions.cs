using System;

namespace restlessmedia.Module.Web
{
  public class HttpCacheOptions
  {
    public HttpCacheOptions(bool isPublic)
    {
      IsPublic = isPublic;
    }

    public HttpCacheOptions()
      : this(true) { }

    public bool IsPublic;

    public DateTime? LastModified;

    public string ETag;

    public string QuotedETag
    {
      get
      {
        const string quote = "\"";

        if (string.IsNullOrWhiteSpace(ETag))
        {
          return quote + quote;
        }

        return string.Concat(quote, ETag, quote);
      }
    }

    public TimeSpan? MaxAge;
  }
}