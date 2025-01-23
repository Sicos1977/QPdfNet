//
// EncryptParameters.cs
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
///     The encryption parameters
/// </summary>
public class EncryptParameters
{
    #region Properties
    [JsonProperty("P", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public string? P { get; set; }

    [JsonProperty("R", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public string? R { get; set; }

    [JsonProperty("V", DefaultValueHandling = DefaultValueHandling.Ignore)] 
    public string? V { get; set; }

    /// <summary>
    ///     Encryption key bit length
    /// </summary>
    [JsonProperty("bits", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Bits { get; set; }

    /// <summary>
    ///     Encryption method for attachments
    /// </summary>
    [JsonProperty("filemethod", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? FileMethod { get; set; }

    /// <summary>
    ///     Encryption key; will be null unless --show-encryption-key was specified
    /// </summary>
    [JsonProperty("key", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Key { get; set; }

    /// <summary>
    ///     Overall encryption method: none, mixed, RC4, AESv2, AESv3
    /// </summary>
    [JsonProperty("method", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Method { get; set; }

    /// <summary>
    ///     Encryption method for streams
    /// </summary>
    [JsonProperty("streammethod", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? StreamMethod { get; set; }

    /// <summary>
    ///     Encryption method for string
    /// </summary>
    [JsonProperty("stringmethod", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? StringMethod { get; set; }
    #endregion
}