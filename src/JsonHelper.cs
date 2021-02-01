using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace restlessmedia.Module.Web
{
  public static class JsonHelper
  {
    public static object Deserialize(Type type, Stream stream)
    {
      using (StreamReader reader = new StreamReader(stream))
      {
        return JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
      }
    }

    public static T Deserialize<T>(Stream stream)
    {
      using (StreamReader reader = new StreamReader(stream))
      {
        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
      }
    }

    public static T Deserialize<T>(string value)
    {
      return JsonConvert.DeserializeObject<T>(value);
    }

    public static string Serialize(object value)
    {
      return JsonConvert.SerializeObject(value, DefaultOptions);
    }

    public static string Serialize<T>(T value)
    {
      return JsonConvert.SerializeObject(value, DefaultOptions);
    }

    public static void Serialize<T>(T value, Stream stream, Encoding encoding = null)
    {
      byte[] data = (encoding ?? Encoding.UTF8).GetBytes(Serialize(value));
      stream.Write(data, 0, data.Length);
    }

    public static JsonSerializerSettings DefaultOptions = new JsonSerializerSettings
    {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      ContractResolver = new CamelCasePropertyNamesContractResolver(),
      Formatting = Formatting.None
    };
  }
}