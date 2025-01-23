//
// QPdfApi.cs
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
using QPdfNet.InteropDotNet;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace QPdfNet.Interop
{
    #region public enum qpdf_log_dest_e
    /// <summary>
    ///     Only used for marshalling
    /// </summary>
    public enum qpdf_log_dest_e 
    {
        /// <summary>
        ///     Default
        /// </summary>
        qpdf_log_dest_default = 0,

        /// <summary>
        ///     Logging to standard output
        /// </summary>
        qpdf_log_dest_stdout = 1,

        /// <summary>
        ///     Logging to error output
        /// </summary>
        qpdf_log_dest_stderr = 2,

        /// <summary>
        ///     Discard any logging
        /// </summary>
        qpdf_log_dest_discard = 3,

        /// <summary>
        ///     Custom logging
        /// </summary>
        qpdf_log_dest_custom = 4
    }
    #endregion

    /// <summary>
    ///     The exported qpdf api signatures.
    /// </summary>
    /// <remarks>
    ///     Please note this is only public for technical reasons (you can't proxy an internal interface).
    ///     It should be considered an internal interface and is NOT part of the public api and may have
    ///     breaking changes between releases.
    /// </remarks>
    public interface IQPdfApiSignatures
    {
        /// <summary>
        ///     Returns the current version
        /// </summary>
        /// <returns></returns>
        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdf_get_qpdf_version))]
        [return: MarshalAs(UnmanagedType.LPTStr)]
        string qpdf_get_qpdf_version();

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_create))]
        IntPtr qpdflogger_create();

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_cleanup))]
        void qpdflogger_cleanup(IntPtr loggerHandle);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_init))]
        IntPtr qpdfjob_init();

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_cleanup))]
        void qpdfjob_cleanup(IntPtr jobHandle);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_set_logger))]
        void qpdfjob_set_logger(IntPtr jobHandle, IntPtr loggerHandle);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_info))]
        void qpdflogger_set_info(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_warn))]
        void qpdflogger_set_warn(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_error))]
        void qpdflogger_set_error(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_save))]
        void qpdflogger_set_save(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_initialize_from_json))]
        int qpdfjob_initialize_from_json(IntPtr jobHandle, string json);

        [RuntimeDllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_run))]
        int qpdfjob_run(IntPtr jobHandle);
    }

    public class QPdfApi
    {
        #region Fields
        private IQPdfApiSignatures? _native;
        #endregion

        #region Properties
        public IQPdfApiSignatures Native
        {
            get
            {
                if (_native != null) 
                    return _native;
                
                Helper.SetPath();
                _native = InteropRuntimeImplementer.CreateInstance<IQPdfApiSignatures>();
                return _native;
            }
        }
        #endregion
    }
}
