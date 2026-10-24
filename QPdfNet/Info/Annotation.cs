//
// Annotation.cs
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
///     Represents a PDF annotation, including its flags, appearance state, and a reference to the underlying annotation object.
/// </summary>
/// <remarks>Used to model annotation data from qpdf JSON output, including the /F annotation flags and appearance
/// state values used by controls such as check boxes and radio buttons.</remarks>
public class Annotation
{
    #region Properties
    /// <summary>
    ///     Annotation flags from /F -- see pdf_annotation_flag_e in qpdf/Constants.h
    /// </summary>
    [JsonPropertyName("annotationflags")]
    public string? AnnotationFlags { get; set; }

    /// <summary>
    ///     Appearance state -- can be used to determine value for checkboxes and radio buttons
    /// </summary>
    [JsonPropertyName("appearancestate")]
    public string? AppearanceState { get; set; }

    /// <summary>
    ///     Reference to the annotation object
    /// </summary>
    [JsonPropertyName("object")]
    public string? Object { get; set; }
    #endregion
}