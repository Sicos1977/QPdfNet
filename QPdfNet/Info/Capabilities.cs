//
// Capabilities.cs
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
///     Encryption capabilities
/// </summary>
public class Capabilities
{
    #region Properties
    /// <summary>
    ///     Allow extraction for accessibility?
    /// </summary>
    [JsonPropertyName("accessibility")]
    public bool Accessibility { get; set; }

    /// <summary>
    ///     Allow extraction?
    /// </summary>
    [JsonPropertyName("extract")]
    public bool Extract { get; set; }

    /// <summary>
    ///     Allow modifying annotations?
    /// </summary>
    [JsonPropertyName("modifyannotations")]
    public bool ModifyAnnotations { get; set; }

    /// <summary>
    ///     Allow all modifications?
    /// </summary>
    [JsonPropertyName("modify")]
    public bool Modify { get; set; }

    /// <summary>
    ///     Allow modifying document assembly?
    /// </summary>
    [JsonPropertyName("modifyassembly")]
    public bool ModifyAssembly { get; set; }

    /// <summary>
    ///     Allow modifying forms?
    /// </summary>
    [JsonPropertyName("modifyforms")]
    public bool ModifyForms { get; set; }

    /// <summary>
    ///     Allow other modifications?
    /// </summary>
    [JsonPropertyName("modifyother")]
    public bool ModifyOther { get; set; }

    /// <summary>
    ///     Allow high resolution printing?
    /// </summary>
    [JsonPropertyName("printhigh")]
    public bool PrintHigh { get; set; }

    /// <summary>
    ///     Allow low resolution printing?
    /// </summary>
    [JsonPropertyName("printlow")]
    public bool PrintLow { get; set; }
    #endregion
}