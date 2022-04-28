using System.Collections.Generic;
using Newtonsoft.Json;

namespace QPdfNet.Output;

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