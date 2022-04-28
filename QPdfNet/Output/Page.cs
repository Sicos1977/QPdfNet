using System.Collections.Generic;
using Newtonsoft.Json;

namespace QPdfNet.Output;

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