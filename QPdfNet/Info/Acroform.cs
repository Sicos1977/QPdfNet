//
// Acroform.cs
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
///     Returns information about form fields used in the PDF
/// </summary>
public class Acroform
{
    #region Properties
    /// <summary>
    ///     Returns the fields
    /// </summary>
    [JsonProperty("fields")]
    public List<object>? Fields { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when the PDF has form fields
    /// </summary>
    [JsonProperty("hasacroform")]
    public bool HasAcroform { get; private set; }

    /// <summary>
    ///     Returns <c>true</c> when appearances are needed
    /// </summary>
    [JsonProperty("needappearances")]
    public bool NeedAppearances { get; private set; }
    #endregion
}