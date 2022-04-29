using Newtonsoft.Json;

namespace QPdfNet.Info;

public class Capabilities
{
    [JsonProperty("accessibility")]
    public bool Accessibility { get; set; }

    [JsonProperty("extract")]
    public bool Extract { get; set; }

    [JsonProperty("moddifyannotations")]
    public bool Moddifyannotations { get; set; }

    [JsonProperty("modify")]
    public bool Modify { get; set; }

    [JsonProperty("modifyassembly")]
    public bool Modifyassembly { get; set; }

    [JsonProperty("modifyforms")]
    public bool Modifyforms { get; set; }

    [JsonProperty("modifyother")]
    public bool Modifyother { get; set; }

    [JsonProperty("printhigh")]
    public bool PrintHigh { get; set; }

    [JsonProperty("printlow")]
    public bool PrintLow { get; set; }
}