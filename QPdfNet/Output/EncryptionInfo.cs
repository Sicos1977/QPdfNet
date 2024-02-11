//
// EncryptionInfo.cs
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

using System;

namespace QPdfNet.Output;

/// <summary>
///     A helper class to parse the information that is returned by the <see cref="Job.ShowEncryption"/>
///     or <see cref="Job.ShowEncryptionKey"/> method
/// </summary>
public class EncryptionInfo
{
    #region Properties
    public int? R { get; }

    public int? P { get; }

    /// <summary>
    ///     Returns the user password
    /// </summary>
    public string? UserPassword { get; }

    /// <summary>
    ///     Returns the used encryption key
    /// </summary>
    public string? EncryptionKey { get; }

    /// <summary>
    ///     Returns <c>true</c> when the supplied password is the owner password
    /// </summary>
    public bool SuppliedPasswordIsOwnerPassword { get; }

    /// <summary>
    ///     Returns <c>true</c> when the supplied password is the user password
    /// </summary>
    public bool SuppliedPasswordIsUserPassword { get; }

    /// <summary>
    ///     Returns <c>true</c> when extraction for accessibility is enabled
    /// </summary>
    public bool ExtractForAccesibility { get; }

    /// <summary>
    ///     Returns <c>true</c> when extraction for any purpose is enabled
    /// </summary>
    public bool ExtractForAnyPurpose { get; }

    /// <summary>
    ///     Returns <c>true</c> when print in low quality is allowed
    /// </summary>
    public bool PrintLowResolution { get; }

    /// <summary>
    ///     Returns <c>true</c> when print in high quality is allowed
    /// </summary>
    public bool PrintHighResulution { get; }

    /// <summary>
    ///     Returns <c>true</c> when document assembly is allowed
    /// </summary>
    public bool ModifyDocumentAssembly { get; }

    /// <summary>
    ///     Returns <c>true</c> when form modification is allowed
    /// </summary>
    public bool ModifyForms { get; }

    /// <summary>
    ///     Returns <c>true</c> when it is allowed to add or change annotations
    /// </summary>
    public bool ModifyAnnotations { get; }

    /// <summary>
    ///     Returns <c>true</c> for any other modification
    /// </summary>
    public bool ModifyOther { get; }

    /// <summary>
    ///     Returns <c>true</c> when anything else can be modified
    /// </summary>
    public bool ModifyAnything { get; }

    /// <summary>
    ///     Returns the used stream encryption method
    /// </summary>
    public string? StreamEncryptionMethod { get; }

    /// <summary>
    ///     Returns the used string encryption method
    /// </summary>
    public string? StringEncryptionMethod { get; }

    /// <summary>
    ///     Returns the used file encryption method
    /// </summary>
    public string? FileEncryptionMethod { get; }
    #endregion

    #region Constructor
    /// <summary>
    ///     Makes this object and sets all it's properties
    /// </summary>
    internal EncryptionInfo(string? output)
    {
        if (output == null)
            return;

        var lines = output.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (line.StartsWith("R ="))
                R = int.Parse(GetValue('=', line));
            else if (line.StartsWith("P ="))
                P = int.Parse(GetValue('=', line));
            else if (line.StartsWith("user password"))
                UserPassword = GetValue('=', line);
            else if (line.StartsWith("Encryption key"))
                EncryptionKey = GetValue('=', line);
            else if (line.StartsWith("Supplied password is owner password"))
                SuppliedPasswordIsOwnerPassword = true;
            else if (line.StartsWith("Supplied password is user password"))
                SuppliedPasswordIsUserPassword = true;
            else if (line.StartsWith("extract for accessibility"))
                ExtractForAccesibility = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("extract for any purpose"))
                ExtractForAnyPurpose = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("print low resolution"))
                PrintLowResolution = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("print high resolution"))
                PrintHighResulution = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("modify document assembly"))
                ModifyDocumentAssembly = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("modify forms"))
                ModifyForms = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("modify annotations"))
                ModifyAnnotations = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("modify other"))
                ModifyOther = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("modify anything"))
                ModifyAnything = !GetValue(':', line).Contains("not allowed");
            else if (line.StartsWith("stream encryption method"))
                StreamEncryptionMethod = GetValue(':', line);
            else if (line.StartsWith("string encryption method"))
                StringEncryptionMethod = GetValue(':', line);
            else if (line.StartsWith("file encryption method"))
                FileEncryptionMethod = GetValue(':', line);
        }
    }
    #endregion

    #region GetValue
    private string GetValue(char splitChar, string line)
    {
        var parts = line.Split(new[] {splitChar}, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 2 ? parts[1].Trim() : string.Empty;
    }
    #endregion
}