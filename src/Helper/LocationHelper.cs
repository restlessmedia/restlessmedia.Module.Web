using restlessmedia.Module.File;
using restlessmedia.Module.File.Configuration;
using System;
using System.Collections.Generic;

namespace restlessmedia.Module.Web.Helper
{
  public class LocationHelper
  {
    public LocationHelper(IFileSettings fileSettings, IFileService fileService)
    {
      _fileSettings = fileSettings ?? throw new ArgumentNullException(nameof(fileSettings));
      _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
    }

    public string GetImageUrl(string path, string fileName, string size)
    {
      if (string.IsNullOrWhiteSpace(size))
      {
        throw new ArgumentNullException(nameof(size));
      }

      IFileSize fileSize = _fileSettings.GetSize(size);

      if (fileSize == null)
      {
        throw new KeyNotFoundException($"Size not found with key '{size}'");
      }

      string url = GetUrl(_fileService.GetUri(path, fileName));

      if (fileSize.IsPreset)
      {
        return $"{url}?preset={fileSize.Name}";
      }

      return $"{url}?width={fileSize.Width}&height={fileSize.Height}&quality={fileSize.Quality}&mode={fileSize.Mode}&scale={fileSize.Scale}&bgcolor={fileSize.BgColor}&anchor={fileSize.Anchor}";
    }

    public string GetImageUrl(string path, string fileName)
    {
      if (string.IsNullOrWhiteSpace(path))
      {
        throw new ArgumentNullException(nameof(path));
      }

      if (string.IsNullOrWhiteSpace(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      return GetUrl(_fileService.GetUri(path, fileName));
    }

    public string GetImageUrl(string path, IFile file)
    {
      if (string.IsNullOrWhiteSpace(path))
      {
        throw new ArgumentNullException(nameof(path));
      }

      if (string.IsNullOrEmpty(file.SystemFileName))
      {
        return null;
      }

      return GetImageUrl(path, file.SystemFileName);
    }

    public string GetImageUrl(string path, IFile file, string size)
    {
      if (string.IsNullOrWhiteSpace(path))
      {
        throw new ArgumentNullException(nameof(path));
      }

      if (file == null)
      {
        throw new ArgumentNullException(nameof(file));
      }

      if (string.IsNullOrWhiteSpace(size))
      {
        throw new ArgumentNullException(nameof(size));
      }

      if (string.IsNullOrEmpty(file.SystemFileName))
      {
        return null;
      }

      return GetImageUrl(path, file.SystemFileName, size);
    }

    private string GetUrl(Uri fileUri)
    {
      string url = fileUri.ToString();

      if (string.IsNullOrEmpty(_fileSettings.CDN))
      {
        return url;
      }

      return string.Concat(_fileSettings.CDN, url);
    }

    private readonly IFileSettings _fileSettings;

    private readonly IFileService _fileService;
  }
}