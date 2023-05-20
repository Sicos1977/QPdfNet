//
// Page.cs
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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace QPdfNet.Info;

/// <summary>
///     Returns the page
/// </summary>
public class Page
{
    #region Properties
    /// <summary>
    ///     Returns information about the content that is found on the page
    /// </summary>
    [JsonProperty("contents")]
    public List<string>? Contents { get; set; }

    /// <summary>
    ///     Returns information about the images found on the page
    /// </summary>
    [JsonProperty("images")]
    public List<Image>? Images { get; set; }

    /// <summary>
    ///     Returns the page label
    /// </summary>
    [JsonProperty("label")]
    public object? Label { get; set; }

    /// <summary>
    ///     Returns the object
    /// </summary>
    [JsonProperty("object")]
    public string? Object { get; set; }

    /// <summary>
    ///     Returns the page outlines
    /// </summary>
    [JsonProperty("outlines")]
    public List<PageOutline>? Outlines { get; set; }

    /// <summary>
    ///     Returns the page number
    /// </summary>
    [JsonProperty("pageposfrom1")]
    public long PageNumber { get; set; }
    #endregion
}