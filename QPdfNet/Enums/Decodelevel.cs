//
// DecodeLevel.cs
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


using System.Runtime.Serialization;

namespace QPdfNet.Enums;

/// <summary>
///     Decode level
/// </summary>
public enum DecodeLevel
{
    /// <summary>
    ///     No value is set
    /// </summary>
    NotSet,

    /// <summary>
    ///     Do not attempt to decode any streams
    /// </summary>
    [EnumMember(Value = "none")] 
    None,

    /// <summary>
    ///     Decode streams filtered with supported generalized filters: /LZWDecode, /FlateDecode, /ASCII85Decode, and
    ///     /ASCIIHexDecode
    /// </summary>
    [EnumMember(Value = "generalized")] 
    Generalized,

    /// <summary>
    ///     In addition to generalized, decode streams with supported non-lossy specialized filters; currently this is just
    ///     /RunLengthDecode
    /// </summary>
    [EnumMember(Value = "specialized")] 
    Specialized,

    /// <summary>
    ///     In addition to generalized and specialized, decode streams with supported lossy filters; currently this is just
    ///     /DCTDecode (JPEG)
    /// </summary>
    [EnumMember(Value = "all")] 
    All
}