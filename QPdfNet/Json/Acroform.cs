#nullable disable
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QPdfNet.Json;

public partial class Acroform
{
    [JsonProperty("fields")]
    public List<object> Fields { get; set; }

    [JsonProperty("hasacroform")]
    public bool Hasacroform { get; set; }

    [JsonProperty("needappearances")]
    public bool Needappearances { get; set; }
}