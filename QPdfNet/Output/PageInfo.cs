using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QPdfNet.Output
{
    public partial class PageInfo
    {
        [JsonProperty("pages")]
        public List<Page> Pages { get; set; }

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

    public class Page
    {
        [JsonProperty("contents")]
        public List<string> Contents { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("label")]
        public object Label { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("outlines")]
        public List<object> Outlines { get; set; }

        [JsonProperty("pageposfrom1")]
        public long Pageposfrom1 { get; set; }
    }

    public class Image
    {
        [JsonProperty("bitspercomponent")]
        public long Bitspercomponent { get; set; }

        [JsonProperty("colorspace")]
        public string Colorspace { get; set; }

        [JsonProperty("decodeparms")]
        public List<object> Decodeparms { get; set; }

        [JsonProperty("filter")]
        public List<string> Filter { get; set; }

        [JsonProperty("filterable")]
        public bool Filterable { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class Parameters
    {
        [JsonProperty("decodelevel")]
        public string Decodelevel { get; set; }
    }

    public partial class PageInfo
    {
        public static PageInfo FromJson(string json) => JsonConvert.DeserializeObject<PageInfo>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PageInfo self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
