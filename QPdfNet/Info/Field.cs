//
// Fields.cs
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
///     The form fields
/// </summary>
public class Field
{
    #region Properties
    /// <summary>
    ///     Alternative name of field -- this is the one usually shown to users
    /// </summary>
    [JsonPropertyName("alternativename")]
    public string? AlternativeName { get; set; }

    /// <summary>
    ///     Reference to the annotation object
    /// </summary>
    [JsonPropertyName("annotation")]
    public Annotation? Annotation { get; set; }

    /// <summary>
    ///     For choices fields, the list of choices presented to the user
    /// </summary>
    [JsonPropertyName("choices")]
    public string? Choices { get; set; }

    /// <summary>
    ///     Default value of field
    /// </summary>
    [JsonPropertyName("defaultvalue")]
    public string? DefaultValue { get; set; }

    /// <summary>
    ///     Form field flags from /Ff -- see pdf_form_field_flag_e in qpdf/Constants.h
    /// </summary>
    [JsonPropertyName("fieldflags")]
    public string? FieldFlags { get; set; }

    /// <summary>
    ///     Field type
    /// </summary>
    [JsonPropertyName("fieldtype")]
    public string? FieldType { get; set; }

    /// <summary>
    ///     Full name of field
    /// </summary>
    [JsonPropertyName("fullname")]
    public string? FullName { get; set; }

    /// <summary>
    ///     Whether field is a checkbox
    /// </summary>
    [JsonPropertyName("ischeckbox")]
    public bool? IsCheckbox { get; set; }

    /// <summary>
    ///     Whether field is a list, combo, or dropdown
    /// </summary>
    [JsonPropertyName("ischoice")]
    public bool? IsChoice { get; set; }

    /// <summary>
    ///     Whether field is a radio button -- buttons in a single group share a parent
    /// </summary>
    [JsonPropertyName("isradiobutton")]
    public bool? IsRadiobutton { get; set; }

    /// <summary>
    ///     Whether field is a text field
    /// </summary>
    [JsonPropertyName("istext")]
    public bool? IsText { get; set; }

    /// <summary>
    ///     Mapping name of field
    /// </summary>
    [JsonPropertyName("mappingname")]
    public string? MappingName { get; set; }

    /// <summary>
    ///     Reference to this form field
    /// </summary>
    [JsonPropertyName("object")]
    public string? Object { get; set; }

    /// <summary>
    ///     Position of containing page numbered from 1
    /// </summary>
    [JsonPropertyName("pageposfrom1")]
    public int PagePosFrom1 { get; set; }

    /// <summary>
    ///     Reference to this field's parent
    /// </summary>
    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    /// <summary>
    ///     Partial name of field
    /// </summary>
    [JsonPropertyName("partialname")]
    public string? PartialName { get; set; }

    /// <summary>
    ///     Field quadding -- number indicating left, center, or right
    /// </summary>
    [JsonPropertyName("quadding")]
    public string? Quadding { get; set; }

    /// <summary>
    ///     Value of field
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
    #endregion
}