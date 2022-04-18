//
// QPdfApi.cs
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

using System;
using System.Runtime.InteropServices;
using QPdfNet.Enums;
using QPdfNet.InteropDotNet;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace QPdfNet.Interop
{
    /// <summary>
    ///     The exported qpdf api signatures.
    /// </summary>
    /// <remarks>
    ///     Please note this is only public for technical reasons (you can't proxy a internal interface).
    ///     It should be considered an internal interface and is NOT part of the public api and may have
    ///     breaking changes between releases.
    /// </remarks>
    public interface IQPdfApiSignatures
    {
        /// <summary>
        ///     Returns the current version
        /// </summary>
        /// <returns></returns>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "qpdf_get_qpdf_version")]
        [return: MarshalAs(UnmanagedType.LPTStr)]
        string GetQPdfVersion();

        /// <summary>
        ///     Runs the given json
        /// </summary>
        /// <returns></returns>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "qpdfjob_run_from_json")]
        int RunFromJSON(string json);

        /// <summary>
        ///     Runs the given json and returns the result from cout and cerr
        /// </summary>
        /// <returns></returns>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "qpdfjob_run_from_json_with_result")]
        int RunFromJSONWithResult(string json, out IntPtr out_result, out IntPtr err_result);

        /// <summary>
        ///     Runs the given json and returns the result from cout and cerr
        /// </summary>
        /// <returns></returns>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "qpdfjob_run_from_json_with_result_free_string")]
        int RunFromJSONWithResultFreeString(IntPtr str);
    }

    public static class QPdfApi
    {
        #region Fields
        private static IQPdfApiSignatures native;
        #endregion

        #region Properties
        public static IQPdfApiSignatures Native
        {
            get
            {
                if (native != null) 
                    return native;
                
                Helper.SetPath();
                native = InteropRuntimeImplementer.CreateInstance<IQPdfApiSignatures>();
                return native;
            }
        }
        #endregion
    }
}