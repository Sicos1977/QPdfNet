//
// Encrypt.cs
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
///     The encryption information
/// </summary>
public class Encrypt
{
    #region Properties
    /// <summary>
    ///     The capabilities of the encryption
    /// </summary>
    [JsonPropertyName("capabilities")]
    public Capabilities? Capabilities { get; set; }

    /// <summary>
    ///     Whether the document is encrypted
    /// </summary>
    [JsonPropertyName("encrypted")]
    public bool Encrypted { get; set; }

    /// <summary>
    ///     Whether supplied password matched owner password; always false for non-encrypted files
    /// </summary>
    [JsonPropertyName("ownerpasswordmatched")]
    public bool OwnerPasswordMatched { get; set; }

    /// <summary>
    ///     Encryption parameters
    /// </summary>
    [JsonPropertyName("parameters")]
    public EncryptParameters? Parameters { get; set; }

    /// <summary>
    ///     Whether supplied password matched user password; always false for non-encrypted files
    /// </summary>
    [JsonPropertyName("userpasswordmatched")]
    public bool UserPasswordMatched { get; set; }
    #endregion
}