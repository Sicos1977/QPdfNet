//
// Constants.cs
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

namespace QPdfNet.Interop;

/// <summary>
///     Description of Constants.
/// </summary>
internal static class Constants
{
    /// <summary>
    ///     The platform-neutral base name of the native qpdf library.
    /// </summary>
    /// <remarks>
    ///     The name is intentionally without a file extension or <c>lib</c> prefix so that the
    ///     platform specific <see cref="QPdfNet.InteropDotNet.ILibraryLoaderLogic.FixUpLibraryName"/>
    ///     implementations can turn it into the correct file name for the current operating system:
    ///     <list type="bullet">
    ///         <item><description>Windows: <c>qpdf30.dll</c></description></item>
    ///         <item><description>Linux: <c>libqpdf30.so</c></description></item>
    ///         <item><description>macOS: <c>libqpdf30.dylib</c></description></item>
    ///     </list>
    /// </remarks>
    public const string QPdfDllName = "qpdf30";
}