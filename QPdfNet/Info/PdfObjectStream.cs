//
// PdfObjectStream.cs
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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QPdfNet.Info;

/// <summary>
///     Represents a stream object from the qpdf JSON output (version 2).
/// </summary>
public class PdfObjectStream
{
    #region Properties
    /// <summary>
    ///     The stream dictionary. The raw value is exposed as a <see cref="JsonElement" /> because a stream
    ///     dictionary can contain any type of data.
    /// </summary>
    [JsonPropertyName("dict")]
    public JsonElement? Dict { get; set; }

    /// <summary>
    ///     The base64 encoded stream data. This is only present when the stream data is requested to be
    ///     included inline in the JSON output. Use <see cref="DataBytes" /> to get the decoded data.
    /// </summary>
    [JsonPropertyName("data")]
    public string? Data { get; set; }

    /// <summary>
    ///     The decoded (base64) stream data, or <c>null</c> when no <see cref="Data" /> is available
    /// </summary>
    [JsonIgnore]
    public byte[]? DataBytes =>
        string.IsNullOrEmpty(Data) ? null : Convert.FromBase64String(Data);
    #endregion
}
