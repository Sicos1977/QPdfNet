//
// Metadata.cs
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
///     Represents the document information dictionary (<c>/Info</c>) of a PDF file,
///     exposing the most common meta-data fields such as title, author and producer,
///     together with per page information such as size, rotation and annotations.
/// </summary>
public class Metadata
{
    #region Properties
    /// <summary>
    ///     The document's title (<c>/Title</c>)
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     The name of the person who created the document (<c>/Author</c>)
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    ///     The subject of the document (<c>/Subject</c>)
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    ///     Keywords associated with the document (<c>/Keywords</c>)
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    ///     The name of the application that created the original document (<c>/Creator</c>)
    /// </summary>
    public string? Application { get; set; }

    /// <summary>
    ///     The name of the application that produced the PDF (<c>/Producer</c>)
    /// </summary>
    public string? Producer { get; set; }

    /// <summary>
    ///     The date and time the document was created (<c>/CreationDate</c>), or <c>null</c> when not set
    /// </summary>
    public System.DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    ///     The date and time the document was most recently modified (<c>/ModDate</c>), or <c>null</c> when not set
    /// </summary>
    public System.DateTimeOffset? ModificationDate { get; set; }

    /// <summary>
    ///     The document's trapped state (<c>/Trapped</c>), e.g. <c>/True</c>, <c>/False</c> or <c>/Unknown</c>
    /// </summary>
    public string? Trapped { get; set; }

    /// <summary>
    ///     The number of pages in the document
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    ///     Information about each page in the document
    /// </summary>
    public System.Collections.Generic.List<PageInfo> Pages { get; set; } = [];
    #endregion
}