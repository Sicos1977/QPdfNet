//
// Helper.cs
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

using System;
using System.Collections.Generic;

namespace QPdfNet.Output;

/// <summary>
///     A class the help parse the output from some QPDF methods
/// </summary>
public static class Helper
{
    #region ParseListAttachments
    /// <summary>
    ///     Helper method to parse the output that is returned by the <see cref="Job.ListAttachments"/>
    ///     method
    /// </summary>
    /// <param name="output">The output of the <see cref="Job.ListAttachments"/> method</param>
    /// <returns>A list with attachment names</returns>
    public static List<string> ParseListAttachments(string? output)
    {
        var result = new List<string>();

        if (output == null)
            return result;

        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var line in lines)
        {
            var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
            result.Add(parts[0].Trim());
        }

        return result;
    }
    #endregion

    #region ParseShowEncryption
    /// <summary>
    ///     Helper method to parse the output that is returns by the <see cref="Job.ShowEncryption"/>
    ///     or <see cref="Job.ShowEncryptionKey"/> method
    /// </summary>
    /// <param name="output">The output of the <see cref="Job.ShowEncryption"/> or <see cref="Job.ShowEncryptionKey"/> method</param>
    /// <returns><see cref="EncryptionInfo"/></returns>
    public static EncryptionInfo ParseShowEncryption(string? output)
    {
        output ??= string.Empty;
        return new EncryptionInfo(output);
    }
    #endregion

    #region ParseShowXref
    /// <summary>
    ///     Helper method to parse the output that is returns by the <see cref="Job.ShowXref"/>
    /// </summary>
    /// <param name="output">The output of the <see cref="Job.ShowXref"/> method</param>
    /// <returns><see cref="XrefInfos"/></returns>
    public static XrefInfos ParseShowXref(string? output)
    {
        output ??= string.Empty;
        return new XrefInfos(output);
    }
    #endregion

    #region ParseCheck
    /// <summary>
    ///     Helper method to parse the output that is returns by the <see cref="Job.Check"/>
    /// </summary>
    /// <param name="output">The output of the <see cref="Job.Check"/> method</param>
    /// <returns><see cref="CheckInfo"/></returns>
    public static CheckInfo ParseCheck(string? output)
    {
        output ??= string.Empty;
        return new CheckInfo(output);
    }
    #endregion
}