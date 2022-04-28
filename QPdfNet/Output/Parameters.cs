using Newtonsoft.Json;

namespace QPdfNet.Output;

public class Parameters
{
    [JsonProperty("decodelevel")]
    public string Decodelevel { get; set; }
}