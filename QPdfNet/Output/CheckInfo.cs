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
using System.Collections.Generic;

namespace QPdfNet.Output;

/// <summary>
///     A helper class to parse the information that is returned by the <see cref="Job.ShowXref"/> method
/// </summary>
public class CheckInfo
{
    #region Properties
    /// <summary>
    ///     Returns the version of the PDF
    /// </summary>
    public string PdfVersion { get; }

    /// <summary>
    ///     Returns <c>true</c> when the PDF file is encrypted
    /// </summary>
    public bool IsEncrypted { get; }

    /// <summary>
    ///     Returns <c>true</c> when the PDF is linearized (optimized for the web)
    /// </summary>
    public bool IsLinearized { get; }

    /// <summary>
    ///     Returns a list with found warnings
    /// </summary>
    public List<string>? Warnings { get; }

    /// <summary>
    ///     Returns <c>true</c> when there are warnings found
    /// </summary>
    public bool HasWarnings => Warnings?.Count > 0;
    #endregion

    #region Constructor
    /// <summary>
    ///     Makes this object and sets all it's needed properties
    /// </summary>
    /// <param name="output"></param>
    internal CheckInfo(string? output)
    {
        PdfVersion = "Unknown";

        if (output == null)
            return;

        var lines = output.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (line.StartsWith("PDF Version"))
                PdfVersion = GetValue(':', line);
            else if (line.StartsWith("File is encrypted"))
                IsEncrypted = true;
            else if (line.StartsWith("File is linearized"))
                IsLinearized = true;
            else if (line.StartsWith("WARNING: "))
            {
                Warnings ??= new List<string>();
                Warnings.Add(line.Replace("WARNING: ", string.Empty));
            }
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