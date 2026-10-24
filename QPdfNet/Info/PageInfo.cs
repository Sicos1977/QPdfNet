//
// PageInfo.cs
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

namespace QPdfNet.Info;

/// <summary>
///     Size, rotation and annotation information about a single page in a PDF document.
/// </summary>
public class PageInfo
{
    #region Properties
    /// <summary>
    ///     The page number, starting from 1
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    ///     The left coordinate of the page's media box
    /// </summary>
    public float Left { get; set; }

    /// <summary>
    ///     The top coordinate of the page's media box
    /// </summary>
    public float Top { get; set; }

    /// <summary>
    ///     The width of the page's media box
    /// </summary>
    public float Width { get; set; }

    /// <summary>
    ///     The height of the page's media box
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    ///     The clockwise rotation of the page in degrees (<c>/Rotate</c>)
    /// </summary>
    public float Rotate { get; set; }

    /// <summary>
    ///     <c>true</c> when the page has one or more annotations (<c>/Annots</c>)
    /// </summary>
    public bool Annotations { get; set; }
    #endregion
}