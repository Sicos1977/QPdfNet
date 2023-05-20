//
// Modify.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2023 Kees van Spelde.
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

// ReSharper disable UnusedMember.Global

using System.Runtime.Serialization;

namespace QPdfNet.Enums;

/// <summary>
///     Values for modify
/// </summary>
public enum Modify
{
    /// <summary>
    ///     No value is set
    /// </summary>
    NotSet,

    /// <summary>
    ///     Options for 128-bit or 256-bit Encryption
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    ///     Allow document assembly only
    /// </summary>
    [EnumMember(Value = "assembly")]
    Assembly,

    /// <summary>
    ///     <see cref="Assembly"/> permissions plus filling in form fields and signing
    /// </summary>
    [EnumMember(Value = "form")]
    Form,

    /// <summary>
    ///    <see cref="Form"/> permissions plus commenting and modifying forms
    /// </summary>
    [EnumMember(Value = "annotate")]
    Annotate,

    /// <summary>
    ///    Allow full document modification
    /// </summary>
    [EnumMember(Value = "all")]
    All
}