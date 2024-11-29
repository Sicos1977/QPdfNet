//
// Pdf.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2024 Kees van Spelde.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QPdfNet.Enums;
using QPdfNet.Loggers;

namespace QPdfNet.Info;

/// <summary>
///     Returns information about the PDF
/// </summary>
public class Pdf
{
    #region Properties
    /// <summary>
    ///     The JSON version
    /// </summary>
    [JsonProperty("version", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public string? Version { get; set; }

    /// <summary>
    ///     The acro form fields
    /// </summary>
    [JsonProperty("acroform", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public AcroForm? AcroForm { get; set; }

    /// <summary>
    ///     The attachments
    /// </summary>
    [JsonProperty("attachments", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public Dictionary<string, Attachment>? Attachments { get; set; }

    /// <summary>
    ///     Encryption information
    /// </summary>
    [JsonProperty("encrypt", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public Encrypt? Encrypt { get; set; }

    /// <summary>
    ///     The object information
    /// </summary>
    [JsonProperty("objectinfo", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public Dictionary<string, Stream>? Objectinfo { get; set; }

    /// <summary>
    ///     The objects
    /// </summary>
    [JsonProperty("objects", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public string? Objects { get; set; }

    /// <summary>
    ///     The outline information
    /// </summary>
    [JsonProperty("outlines", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public List<PdfOutline>? Outlines { get; set; }

    /// <summary>
    ///     The page labels
    /// </summary>
    [JsonProperty("pagelabels", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public List<PageLabel>? PageLabels { get; set; }

    /// <summary>
    ///     The pages of the document
    /// </summary>
    [JsonProperty("pages", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public List<Page>? Pages { get; set; }

    /// <summary>
    ///     The parameters
    /// </summary>
    [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public Parameters? Parameters { get; set; }
    #endregion

    #region FromFile
    /// <summary>
    ///     Returns a <c>Pdf</c> object with information about the given <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Pdf? FromFile(string fileName)
    {
        Logger.LogInformation($"Getting information from PDF '{fileName}'");

        var result = new Job().InputFile(fileName).Json().Run(out var output, out var data);

        if (result != ExitCode.Success)
            throw new Exception(output);

        if (output != null)
            return JsonConvert.DeserializeObject<Pdf>(Encoding.ASCII.GetString(data!), new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal } }
            });

        return null;
    }
    #endregion
}