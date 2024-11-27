#nullable disable
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QPdfNet.Json;

public partial class NodeResponse
{
    [JsonProperty("acroform")]
    public Acroform Acroform { get; set; }

    [JsonProperty("attachments")]
    public Attachments Attachments { get; set; }

    [JsonProperty("encrypt")]
    public Encrypt Encrypt { get; set; }

    [JsonProperty("objectinfo")]
    public Dictionary<string, Objectinfo> Objectinfo { get; set; }

    [JsonProperty("objects")]
    public Objects Objects { get; set; }

    [JsonProperty("outlines")]
    public List<NodeResponseOutline> Outlines { get; set; }

    [JsonProperty("pagelabels")]
    public List<Pagelabel> Pagelabels { get; set; }

    [JsonProperty("pages")]
    public List<Page> Pages { get; set; }

    [JsonProperty("parameters")]
    public NodeResponseParameters Parameters { get; set; }

    [JsonProperty("version")]
    public long Version { get; set; }
}