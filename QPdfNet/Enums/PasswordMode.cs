//
// PasswordMode.cs
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

// ReSharper disable UnusedMember.Global

using System.Runtime.Serialization;

namespace QPdfNet.Enums;

/// <summary>
///     This option can be used to fine-tune how qpdf interprets Unicode (non-ASCII) password strings passed on the command
///     line. With the exception of the hex-bytes mode, these only apply to passwords provided when encrypting files. The
///     hex-bytes mode also applies to passwords specified for reading files. For additional discussion of the supported
///     password modes and when you might want to use them, see Unicode Passwords
/// </summary>

public enum PasswordMode
{
    /// <summary>
    ///     No value is set
    /// </summary>
    NotSet,

    /// <summary>
    ///     Automatically determine whether the specified password is a properly encoded Unicode (UTF-8) string, and transcode
    ///     it as required by the PDF spec based on the type of encryption being applied. On Windows starting with version
    ///     8.4.0, and on almost all other modern platforms, incoming passwords will be properly encoded in UTF-8, so this is
    ///     almost always what you want.
    /// </summary>
    [EnumMember(Value = "auto")] 
    Auto,

    /// <summary>
    ///     ells qpdf that the incoming password is UTF-8, overriding whatever its automatic detection determines. The only
    ///     difference between this mode and auto is that qpdf will fail with an error message if the password is not valid
    ///     UTF-8 instead of falling back to bytes mode with a warning.
    /// </summary>
    [EnumMember(Value = "unicode")] 
    Unicode,

    /// <summary>
    ///     Interpret the password as a literal byte string. For non-Windows platforms, this is what versions of qpdf prior to
    ///     8.4.0 did. For Windows platforms, there is no way to specify strings of binary data on the command line directly,
    ///     but you can use a @filename option or --password-file to do it, in which case this option forces qpdf to respect
    ///     the string of bytes as provided. Note that this option may cause you to encrypt PDF files with passwords that will
    ///     not be usable by other readers.
    /// </summary>
    [EnumMember(Value = "bytes")] 
    Bytes,

    /// <summary>
    ///     Interpret the password as a hex-encoded string. This provides a way to pass binary data as a password on all
    ///     platforms including Windows. As with bytes, this option may allow creation of files that can’t be opened by other
    ///     readers. This mode affects qpdf’s interpretation of passwords specified for decrypting files as well as for
    ///     encrypting them. It makes it possible to specify strings that are encoded in some manner other than the system’s
    ///     default encoding.
    /// </summary>
    [EnumMember(Value = "hex-bytes")] 
    HexBytes
}