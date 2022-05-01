//
// EncryptParameters.cs
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

using Newtonsoft.Json;

namespace QPdfNet.Info;

public class EncryptParameters
{
    #region Properties
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("P")]
    public long P { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("R")]
    public long R { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("V")]
    public long V { get; private set; }

    /// <summary>
    ///     Returns the amounts of bits used with the encryption
    /// </summary>
    [JsonProperty("bits")]
    public long Bits { get; set; }

    /// <summary>
    ///     Returns the file method used
    /// </summary>
    [JsonProperty("filemethod")]
    public string? FileMethod { get; private set; }

    /// <summary>
    ///     Returns the encryption key
    /// </summary>
    [JsonProperty("key")]
    public object? Key { get; private set; }

    /// <summary>
    ///     Returns the encryption method used
    /// </summary>
    [JsonProperty("method")]
    public string? Method { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("streammethod")]
    public string? StreamMethod { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("stringmethod")]
    public string? StringMethod { get; private set; }
    #endregion
}