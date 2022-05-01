//
// Capabilities.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2022 Kees van Spelde.
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

public class Capabilities
{
    /// <summary>
    ///     Returns <c>true </c> when accessibility is restricted (usually ignored)
    /// </summary>
    [JsonProperty("accessibility")]
    public bool Accessibility { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> text/graphic extraction is restricted
    /// </summary>
    [JsonProperty("extract")]
    public bool Extract { get; private set; }

    /// <summary>
    ///     Returns <c>tru</c> commenting/filling form fields is restricted
    /// </summary>
    [JsonProperty("moddifyannotations")]
    public bool ModifyAnnotations { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when modification of the PDF is restricted
    /// </summary>
    [JsonProperty("modify")]
    public bool Modify { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when document assembly is restricted
    /// </summary>
    [JsonProperty("modifyassembly")]
    public bool ModifyAssembly { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when form field filling is restricted
    /// </summary>
    [JsonProperty("modifyforms")]
    public bool ModifyForms { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when all other modifications are restricted
    /// </summary>
    [JsonProperty("modifyother")]
    public bool ModifyOther { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when printing is restricted to high quality
    /// </summary>
    [JsonProperty("printhigh")]
    public bool PrintHigh { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when printing is restricted to low quality
    /// </summary>
    [JsonProperty("printlow")]
    public bool PrintLow { get; private set; }
}