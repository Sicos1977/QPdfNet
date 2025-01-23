//
// MarshalHelper.cs
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

using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable UnusedMember.Global

namespace QPdfNet.Interop
{
    internal static unsafe class MarshalHelper
    {
        #region Fields
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false);
        #endregion

        #region StringToPtr
        internal static IntPtr StringToPtr(string value, Encoding? encoding = null)
        {
            encoding ??= DefaultEncoding;
            var length = encoding.GetByteCount(value);

            // The encoded value is null terminated that's the reason for the '+1'.
            var encodedValue = new byte[length + 1];
            encoding.GetBytes(value, 0, value.Length, encodedValue, 0);
            var handle = Marshal.AllocHGlobal(new IntPtr(encodedValue.Length));
            Marshal.Copy(encodedValue, 0, handle, encodedValue.Length);
            return handle;
        }
        #endregion

        #region PtrToString
        internal static string? PtrToString(IntPtr handle, Encoding? encoding = null)
        {
            encoding ??= DefaultEncoding;
            if (IntPtr.Zero == handle) 
                return null;

            var length = StrLength(handle);
            return new string((sbyte*)handle.ToPointer(), 0, length, encoding);
        }
        #endregion

        #region StrLength
        /// <summary>
        ///		Gets the number of bytes in a null terminated byte array.
        /// </summary>
        internal static int StrLength(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                return 0;

            var ptr = (byte*)handle.ToPointer();
            var length = 0;
            while (*(ptr + length) != 0)
            {
                length++;
            }
            return length;
        }
        #endregion
    }
}
