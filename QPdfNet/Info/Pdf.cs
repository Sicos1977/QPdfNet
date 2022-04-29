using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QPdfNet.Enums;

namespace QPdfNet.Info;

/// <summary>
///     Returns information about the PDF
/// </summary>
public class Pdf
{
    #region Properties
    [JsonProperty("acroform")]
    public Acroform? Acroform { get; private set; }

    [JsonProperty("attachments")]
    public Attachments Attachments { get; private set; }

    [JsonProperty("encrypt")]
    public Encrypt Encrypt { get; private set; }

    [JsonProperty("objectinfo")]
    public Dictionary<string, Objectinfo>? Objectinfo { get; set; }

    [JsonProperty("objects")]
    public Objects Objects { get; set; }

    [JsonProperty("outlines")]
    public List<object>? Outlines { get; set; }

    [JsonProperty("pagelabels")]
    public List<object>? PageLabels { get; private set; }

    /// <summary>
    ///     Returns the pages of the PDF
    /// </summary>
    [JsonProperty("pages")]
    public List<Page>? Pages { get; private set; }

    /// <summary>
    ///     Returns the parameters used
    /// </summary>
    [JsonProperty("parameters")]
    public Parameters? Parameters { get; private set; }
    #endregion

    #region FromFile
    public static Pdf? FromFile(string fileName)
    {
        var result = new Job().InputFile(fileName).Json().Run(out var output);

        if (result != ExitCode.Success)
            throw new Exception(output);

        if (output != null)
            return JsonConvert.DeserializeObject<Pdf>(output, new JsonSerializerSettings()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                }
            });

        return null;
    }
    #endregion
}