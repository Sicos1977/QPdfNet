#nullable disable
using Newtonsoft.Json;

namespace QPdfNet.Json;

public partial class Objectinfo
{
    [JsonProperty("stream")]
    public Stream Stream { get; set; }
}