//
// Capabilities.cs
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

using Newtonsoft.Json;

namespace QPdfNet.Info;

/// <summary>
///     Encryption capabilities
/// </summary>
public class Capabilities
{
    #region Properties
    /// <summary>
    ///     Allow extraction for accessibility?
    /// </summary>
    [JsonProperty("accessibility", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Accessibility { get; set; }

    /// <summary>
    ///     Allow extraction?
    /// </summary>
    [JsonProperty("extract", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Extract { get; set; }

    /// <summary>
    ///     Allow modifying annotations?
    /// </summary>
    [JsonProperty("moddifyannotations", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool ModdifyAnnotations { get; set; }

    /// <summary>
    ///     Allow all modifications?
    /// </summary>
    [JsonProperty("modify", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Modify { get; set; }

    /// <summary>
    ///     Allow modifying document assembly?
    /// </summary>
    [JsonProperty("modifyassembly", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool ModifyAssembly { get; set; }

    /// <summary>
    ///     Allow modifying forms?
    /// </summary>
    [JsonProperty("modifyforms", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool ModifyForms { get; set; }

    /// <summary>
    ///     Allow other modifications?
    /// </summary>
    [JsonProperty("modifyother", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool ModifyOther { get; set; }

    /// <summary>
    ///     Allow high resolution printing?
    /// </summary>
    [JsonProperty("printhigh", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool PrintHigh { get; set; }

    /// <summary>
    ///     Allow low resolution printing?
    /// </summary>
    [JsonProperty("printlow", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool PrintLow { get; set; }
    #endregion
}