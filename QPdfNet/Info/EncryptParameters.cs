using Newtonsoft.Json;

namespace QPdfNet.Info;

public class EncryptParameters
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
    public string FileMethod { get; set; }

    [JsonProperty("key")]
    public object Key { get; set; }

    [JsonProperty("method")]
    public string Method { get; set; }

    [JsonProperty("streammethod")]
    public string StreamMethod { get; set; }

    [JsonProperty("stringmethod")]
    public string StringMethod { get; set; }
}