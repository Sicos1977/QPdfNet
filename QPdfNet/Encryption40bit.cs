//
// Encryption40Bit.cs
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
using QPdfNet.Enums;
using QPdfNet.Interfaces;

namespace QPdfNet;

/// <summary>
///     Use 40 bit encryption
/// </summary>
public class Encryption40Bit : IEncryption
{
    #region Properties
    [JsonPropertyName("annotate")]
    public string Annotate { get; private set; }

    [JsonPropertyName("extract")]
    public string Extract { get; private set; }

    [JsonPropertyName("modify")]
    public Modify Modify { get; private set; }

    [JsonPropertyName("print")]
    public Print Print { get; private set; }
    #endregion

    #region Constructor
    /// <summary>
    ///     Encrypt the PDF with 40 bit encryption
    /// </summary>
    /// <param name="annotate">Restrict comments, filling forms, and signing</param>
    /// <param name="extract">Restrict text/graphic extraction</param>
    /// <param name="modify">Restrict document <see cref="Modify"/></param>
    /// <param name="print">Restrict <see cref="Print"/></param>
    public Encryption40Bit(
        bool annotate = true, 
        bool extract = true, 
        Modify modify = Modify.All, 
        Print print = Print.Full)
    {
        Annotate = annotate ? "y" : "n";
        Extract = extract ? "y" : "n";
        Modify = modify;
        Print = print;
    }
    #endregion
}