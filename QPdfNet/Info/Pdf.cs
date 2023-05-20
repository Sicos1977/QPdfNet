//
// Pdf.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2023 Kees van Spelde.
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
    ///     <see cref="Acroform"/>
    /// </summary>
    [JsonProperty("acroform")]
    public Acroform? Acroform { get; private set; }

    /// <summary>
    ///     Returns a list with the attachments
    /// </summary>
    [JsonProperty("attachments")]
    public Dictionary<string, AttachmentProperties>? Attachments { get; private set; }

    /// <summary>
    ///     Returns information about PDF encryption
    /// </summary>
    [JsonProperty("encrypt")]
    public Encrypt? Encrypt { get; private set; }

    /// <summary>
    ///     Returns information about the objects in the PDF
    /// </summary>
    [JsonProperty("objectinfo")]
    public Dictionary<string, Objectinfo>? Objectinfo { get; set; }

    //[JsonProperty("objects")]
    //public List<string>? Objects { get; set; }

    [JsonProperty("outlines")]
    public List<object>? Outlines { get; set; }

    /// <summary>
    ///     Returns the used page labels in the PDF
    /// </summary>
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
            return JsonConvert.DeserializeObject<Pdf>(Encoding.ASCII.GetString(data!), new JsonSerializerSettings()
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