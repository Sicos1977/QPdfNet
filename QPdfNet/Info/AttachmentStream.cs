//
// AttachmentStream.cs
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

using System.Text.Json.Serialization;

namespace QPdfNet.Info;

/// <summary>
///     Information about an embedded file stream of an <see cref="Attachment" /> (qpdf JSON output version 2)
/// </summary>
public class AttachmentStream
{
    #region Properties
    /// <summary>
    ///     The checksum of the embedded file stream, or null if none
    /// </summary>
    [JsonPropertyName("checksum")]
    public string? Checksum { get; set; }

    /// <summary>
    ///     The creation date of the embedded file stream, or null if none
    /// </summary>
    [JsonPropertyName("creationdate")]
    public string? CreationDate { get; set; }

    /// <summary>
    ///     The mime type of the embedded file stream, or null if none
    /// </summary>
    [JsonPropertyName("mimetype")]
    public string? MimeType { get; set; }

    /// <summary>
    ///     The modification date of the embedded file stream, or null if none
    /// </summary>
    [JsonPropertyName("modificationdate")]
    public string? ModificationDate { get; set; }
    #endregion
}
