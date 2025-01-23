//
// Image.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2025 Kees van Spelde.
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
///    The image data
/// </summary>
public class Image
{
    #region Properties
    /// <summary>
    ///     Bits per component
    /// </summary>
    [JsonProperty("bitspercomponent", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Bitspercomponent { get; set; }
    
    /// <summary>
    ///     Color space
    /// </summary>
    [JsonProperty("colorspace", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Colorspace { get; set; }

    /// <summary>
    ///     Decode parameters for image data
    /// </summary>
    [JsonProperty("decodeparms", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<string>? Decodeparms { get; set; }

    /// <summary>
    ///     Filters applied to image data
    /// </summary>
    [JsonProperty("filter", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<string>? Filter { get; set; }

    /// <summary>
    ///     Whether image data can be decoded using the decode level qpdf was invoked with
    /// </summary>
    [JsonProperty("filterable", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Filterable { get; set; }

    /// <summary>
    ///     Image height
    /// </summary>
    [JsonProperty("height", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public double? Height { get; set; }

    /// <summary>
    ///     Image width
    /// </summary>
    [JsonProperty("width", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public double? Width { get; set; }

    /// <summary>
    ///     Name of image in XObject table
    /// </summary>
    [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Name { get; set; }

    /// <summary>
    ///     Reference to image stream
    /// </summary>
    [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Object { get; set; }
    #endregion
}