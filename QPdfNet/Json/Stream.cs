#nullable disable
using Newtonsoft.Json;

namespace QPdfNet.Json;

public partial class Stream
{
    [JsonProperty("filter")]
    public Filter? Filter { get; set; }

    [JsonProperty("is")]
    public bool Is { get; set; }

    [JsonProperty("length")]
    public long? Length { get; set; }
}