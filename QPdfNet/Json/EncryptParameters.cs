#nullable disable
using Newtonsoft.Json;

namespace QPdfNet.Json;

public partial class EncryptParameters
{
    [JsonProperty("P")]
    public long P { get; set; }

    [JsonProperty("R")]
    public long R { get; set; }

    [JsonProperty("V")]
    public long V { get; set; }

    [JsonProperty("bits")]
    public long Bits { get; set; }

    [JsonProperty("filemethod")]
    public string Filemethod { get; set; }

    [JsonProperty("key")]
    public object Key { get; set; }

    [JsonProperty("method")]
    public string Method { get; set; }

    [JsonProperty("streammethod")]
    public string Streammethod { get; set; }

    [JsonProperty("stringmethod")]
    public string Stringmethod { get; set; }
}