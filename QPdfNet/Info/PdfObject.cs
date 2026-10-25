//
// PdfObject.cs
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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace QPdfNet.Info;

/// <summary>
///     Represents a single object from the qpdf JSON output (version 2). An object either has a
///     <see cref="Value" /> (for non-stream objects) or a <see cref="Stream" /> (for stream objects).
/// </summary>
public class PdfObject
{
    #region Properties
    /// <summary>
    ///     The value of the object. This is set for non-stream objects. The raw value is exposed as a
    ///     <see cref="JsonElement" /> because a PDF object can contain any type of data.
    /// </summary>
    [JsonPropertyName("value")]
    public JsonElement? Value { get; set; }

    /// <summary>
    ///     The stream of the object. This is set for stream objects.
    /// </summary>
    [JsonPropertyName("stream")]
    public PdfObjectStream? Stream { get; set; }

    /// <summary>
    ///     Returns <c>true</c> when the object is a stream object
    /// </summary>
    [JsonIgnore]
    public bool IsStream => Stream != null;
    #endregion
}
