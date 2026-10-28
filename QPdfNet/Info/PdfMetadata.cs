//
// PdfMetadata.cs
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

namespace QPdfNet.Info;

/// <summary>
///     Represents the values stored inside a PDF document information dictionary (<c>/Info</c>).
/// </summary>
internal class PdfMetadata
{
    /// <summary>
    ///     The document title (<c>/Title</c>).
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     The author of the document (<c>/Author</c>).
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    ///     The subject of the document (<c>/Subject</c>).
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    ///     The keywords associated with the document (<c>/Keywords</c>).
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    ///     The document creator (<c>/Creator</c>).
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    ///     The PDF producer (<c>/Producer</c>).
    /// </summary>
    public string? Producer { get; set; }

    /// <summary>
    ///     The creation date (<c>/CreationDate</c>), serialized as a PDF date string.
    /// </summary>
    public string? CreationDate { get; set; }

    /// <summary>
    ///     The modification date (<c>/ModDate</c>), serialized as a PDF date string.
    /// </summary>
    public string? ModDate { get; set; }
}