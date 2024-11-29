//
// Page.cs
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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace QPdfNet.Info;

/// <summary>
///     The page data
/// </summary>
public class Page
{
    #region Properties
    /// <summary>
    ///     Reference to each content stream
    /// </summary>
    [JsonProperty("contents", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<string>? Contents { get; set; }

    /// <summary>
    ///     List of images on the page
    /// </summary>
    [JsonProperty("images", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public List<Image>? Images { get; set; }

    /// <summary>
    ///     Page label dictionary, or null if none
    /// </summary>
    [JsonProperty("label", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Label { get; set; }

    /// <summary>
    ///     Reference to original page object
    /// </summary>
    [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Object { get; set; }
    
    /// <summary>
    ///     Outlines that target this page
    /// </summary>
    [JsonProperty("outlines", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public List<PageOutline>? Outlines { get; set; }

    /// <summary>
    ///     Position of page in document numbering from 1
    /// </summary>
    [JsonProperty("pageposfrom1", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int? PagePosFrom1 { get; set; }
    #endregion
}