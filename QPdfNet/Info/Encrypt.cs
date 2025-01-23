//
// Encrypt.cs
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

using Newtonsoft.Json;

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
    [JsonProperty("capabilities", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public Capabilities? Capabilities { get; set; }

    /// <summary>
    ///     Whether the document is encrypted
    /// </summary>
    [JsonProperty("encrypted", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Encrypted { get; set; }

    /// <summary>
    ///     Whether supplied password matched owner password; always false for non-encrypted files
    /// </summary>
    [JsonProperty("ownerpasswordmatched", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool OwnerPasswordMatched { get; set; }

    /// <summary>
    ///     Encryption parameters
    /// </summary>
    [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public EncryptParameters? Parameters { get; set; }

    /// <summary>
    ///     Whether supplied password matched user password; always false for non-encrypted files
    /// </summary>
    [JsonProperty("userpasswordmatched", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool UserPasswordMatched { get; set; }
    #endregion
}