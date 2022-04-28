using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QPdfNet.Output
{
    public partial class PageInfo
    {
        [JsonProperty("pages")]
        public List<Page>? Pages { get; set; }

        [JsonProperty("parameters")]
        public Parameters? Parameters { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

    public partial class PageInfo
    {
        public static PageInfo? FromJson(string json) => JsonConvert.DeserializeObject<PageInfo>(json, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings? Settings = new()
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
