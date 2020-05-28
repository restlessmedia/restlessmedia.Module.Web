using restlessmedia.Test;
using System;
using Xunit;

namespace restlessmedia.Module.Web.Tests.Extensions
{
  public class UriExtensionsTests
  {
    public UriExtensionsTests()
    {
      _uriValue = "https://www.restlessmedia.co.uk/files/test.csv";
      Uri.TryCreate(_uriValue, UriKind.Absolute, out _uri);
    }

    [Fact]
    public void FileNameNoExtension()
    {
      _uri.FileNameNoExtension().MustBe("test");
    }

    [Fact]
    public void FileName()
    {
      _uri.FileName().MustBe("test.csv");
    }

    [Fact]
    public void Extension()
    {
      _uri.Extension().MustBe(".csv");
    }

    [Fact]
    public void Extension_string()
    {
      _uriValue.Extension().MustBe(".csv");
    }

    [Fact]
    public void AddParam()
    {
      _uri.AddParam("foo", "bar").ToString().MustBe("https://www.restlessmedia.co.uk/files/test.csv?foo=bar");
    }

    private readonly Uri _uri;

    private readonly string _uriValue;
  }
}