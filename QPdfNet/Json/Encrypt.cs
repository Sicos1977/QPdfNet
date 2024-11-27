#nullable disable
using Newtonsoft.Json;

namespace QPdfNet.Json;

public partial class Encrypt
{
    [JsonProperty("capabilities")]
    public Capabilities Capabilities { get; set; }

    [JsonProperty("encrypted")]
    public bool Encrypted { get; set; }

    [JsonProperty("ownerpasswordmatched")]
    public bool Ownerpasswordmatched { get; set; }

    [JsonProperty("parameters")]
    public EncryptParameters Parameters { get; set; }

    [JsonProperty("userpasswordmatched")]
    public bool Userpasswordmatched { get; set; }
}