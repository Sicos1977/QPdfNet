//
// Pdf.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2026 Kees van Spelde.
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
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using QPdfNet.Enums;
using QPdfNet.Loggers;
using QPdfNet.Serialization;

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
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    ///     The acro form fields
    /// </summary>
    [JsonPropertyName("acroform")] 
    public AcroForm? AcroForm { get; set; }

    /// <summary>
    ///     The attachments
    /// </summary>
    [JsonPropertyName("attachments")] 
    public Dictionary<string, Attachment>? Attachments { get; set; }

    /// <summary>
    ///     Encryption information
    /// </summary>
    [JsonPropertyName("encrypt")] 
    public Encrypt? Encrypt { get; set; }

    /// <summary>
    ///     The object information
    /// </summary>
    [JsonPropertyName("objectinfo")] 
    public Dictionary<string, Stream>? Objectinfo { get; set; }

    /// <summary>
    ///     The objects
    /// </summary>
    [JsonPropertyName("objects")] 
    public string? Objects { get; set; }

    /// <summary>
    ///     The outline information
    /// </summary>
    [JsonPropertyName("outlines")] 
    public List<PdfOutline>? Outlines { get; set; }

    /// <summary>
    ///     The page labels
    /// </summary>
    [JsonPropertyName("pagelabels")] 
    public List<PageLabel>? PageLabels { get; set; }

    /// <summary>
    ///     The pages of the document
    /// </summary>
    [JsonPropertyName("pages")] 
    public List<Page>? Pages { get; set; }

    /// <summary>
    ///     The parameters
    /// </summary>
    [JsonPropertyName("parameters")] 
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
            return JsonSerializer.Deserialize<Pdf>(Encoding.ASCII.GetString(data!), JsonOptions.Deserialize);

        return null;
    }
    #endregion

    #region ContainsJavaScript
    /// <summary>
    ///     Returns <c>true</c> when the PDF contains JavaScript, otherwise <c>false</c>
    /// </summary>
    /// <param name="fileName">The PDF file to check</param>
    /// <returns><c>true</c> when JavaScript is found</returns>
    /// <exception cref="Exception">Thrown when the qpdf operation fails or the file cannot be processed</exception>
    public static bool ContainsJavaScript(string fileName)
    {
        Logger.LogInformation($"Checking if PDF '{fileName}' contains JavaScript");

        var result = new Job().InputFile(fileName).Json().Run(out var output, out var data);

        if (result != ExitCode.Success)
            throw new Exception(output);

        if (data == null)
            return false;

        var json = Encoding.ASCII.GetString(data);
        return json.IndexOf("javascript", StringComparison.OrdinalIgnoreCase) >= 0;
    }
    #endregion
}