//
// EncryptionInfo.cs
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

using System;
using System.Collections.Generic;

namespace QPdfNet.Output;

/// <summary>
///     A helper class to parse the information that is returned by the <see cref="Job.ShowXref"/> method
/// </summary>
public class XrefInfos : List<XrefInfo>
{
    #region Constructor
    /// <summary>
    ///     Makes this object and sets all it's needed properties
    /// </summary>
    /// <param name="output"></param>
    internal XrefInfos(string? output)
    {
        if (output == null)
            return;

        var lines = output.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var parts = line.Trim().Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            var subParts = parts[0].Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
            var id = subParts[0].Trim();
            var state = subParts[1].Trim();
            subParts = parts[1].Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries);
            var offset = subParts[1].Trim();
            Add(new XrefInfo(id, state, long.Parse(offset)));
        }
    }
    #endregion
}

/// <summary>
///     A helper class to parse the information that is returned by the <see cref="Job.ShowXref"/> method
/// </summary>
public class XrefInfo
{
    #region Properties
    /// <summary>
    ///     Returns the id
    /// </summary>
    public string Id { get; }

    /// <summary>
    ///     Returns the state
    /// </summary>
    public string State { get; }

    /// <summary>
    ///     Returns the offset
    /// </summary>
    public long Offset { get; }
    #endregion

    #region Constructor
    /// <summary>
    ///     Makes this object and sets all it's properties
    /// </summary>
    internal XrefInfo(string id, string state, long offset)
    {
        Id = id;
        State = state;
        Offset = offset;
    }
    #endregion
}