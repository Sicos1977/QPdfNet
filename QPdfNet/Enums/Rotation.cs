//
// Rotation.cs
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

namespace QPdfNet.Enums;

/// <summary>
///     Rotation angle
/// </summary>
public enum Rotation
{
    /// <summary>
    ///     No value is set
    /// </summary>
    NotSet,

    /// <summary>
    ///     0 degrees clockwise
    /// </summary>
    Rotate0,

    /// <summary>
    ///     90 degrees clockwise
    /// </summary>
    Rotate90,

    /// <summary>
    ///     90 degrees counter clockwise
    /// </summary>
    RotateMinus90,

    /// <summary>
    ///     180 degrees clockwise
    /// </summary>
    Rotate180,

    /// <summary>
    ///     180 degrees counter clockwise
    /// </summary>
    RotateMinus180,

    /// <summary>
    ///     270 degrees clockwise
    /// </summary>
    Rotate270,

    /// <summary>
    ///     270 degrees counter clockwise
    /// </summary>
    RotateMinus270
}