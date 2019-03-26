using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace restlessmedia.Module.Web
{
  public class JsonOptions
  {
    public static JsonSerializerSettings Default = new JsonSerializerSettings
    {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      ContractResolver = new CamelCasePropertyNamesContractResolver(),
      Formatting = Formatting.None
    };
  }
}