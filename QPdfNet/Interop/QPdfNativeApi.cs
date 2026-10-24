//
// QPdfNativeApi.cs
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

#if NET6_0_OR_GREATER
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using QPdfNet.InteropDotNet;
using QPdfNet.Loggers;
using File = System.IO.File;

// ReSharper disable InconsistentNaming

namespace QPdfNet.Interop
{
    /// <summary>
    ///     A standard P/Invoke based implementation of <see cref="IQPdfApiSignatures" /> used on modern
    ///     .NET (.NET 6 and later). It relies on the built-in <see cref="DllImportAttribute" /> marshaller
    ///     instead of the custom <see cref="InteropDotNet" /> reflection-emit loader, which makes it
    ///     trimming and NativeAOT friendly.
    /// </summary>
    /// <remarks>
    ///     A <see cref="NativeLibrary.SetDllImportResolver(Assembly, DllImportResolver)" /> callback is
    ///     installed so the native qpdf library is resolved from the RID-specific
    ///     <c>runtimes/&lt;rid&gt;/native</c> folder first and then from the legacy <c>x64</c>/<c>x86</c>
    ///     folder, matching the behaviour of the custom loader. When the resolver returns
    ///     <see cref="IntPtr.Zero" /> the default .NET resolution (including automatic
    ///     <c>runtimes/&lt;rid&gt;/native</c> resolution from <c>deps.json</c>) still applies.
    /// </remarks>
    internal sealed class QPdfNativeApi : IQPdfApiSignatures
    {
        #region IQPdfApiSignatures
        public string qpdf_get_qpdf_version() => QPdfNativeMethods.qpdf_get_qpdf_version();

        public IntPtr qpdflogger_create() => QPdfNativeMethods.qpdflogger_create();

        public void qpdflogger_cleanup(IntPtr loggerHandle) => QPdfNativeMethods.qpdflogger_cleanup(loggerHandle);

        public IntPtr qpdfjob_init() => QPdfNativeMethods.qpdfjob_init();

        public void qpdfjob_cleanup(IntPtr jobHandle) => QPdfNativeMethods.qpdfjob_cleanup(jobHandle);

        public void qpdfjob_set_logger(IntPtr jobHandle, IntPtr loggerHandle) => QPdfNativeMethods.qpdfjob_set_logger(jobHandle, loggerHandle);

        public void qpdflogger_set_info(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata) => QPdfNativeMethods.qpdflogger_set_info(loggerHandle, destination, callBackHandler, udata);

        public void qpdflogger_set_warn(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata) => QPdfNativeMethods.qpdflogger_set_warn(loggerHandle, destination, callBackHandler, udata);

        public void qpdflogger_set_error(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata) => QPdfNativeMethods.qpdflogger_set_error(loggerHandle, destination, callBackHandler, udata);

        public void qpdflogger_set_save(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata) => QPdfNativeMethods.qpdflogger_set_save(loggerHandle, destination, callBackHandler, udata);

        public int qpdfjob_initialize_from_json(IntPtr jobHandle, IntPtr json) => QPdfNativeMethods.qpdfjob_initialize_from_json(jobHandle, json);

        public int qpdfjob_run(IntPtr jobHandle) => QPdfNativeMethods.qpdfjob_run(jobHandle);
        #endregion
    }

    /// <summary>
    ///     The raw <see cref="DllImportAttribute" /> declarations for the native qpdf library together with
    ///     the custom import resolver.
    /// </summary>
    internal static class QPdfNativeMethods
    {
        #region Constructor
        static QPdfNativeMethods()
        {
            NativeLibrary.SetDllImportResolver(typeof(QPdfNativeMethods).Assembly, Resolve);
        }
        #endregion

        #region Resolve
        private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (!string.Equals(libraryName, Constants.QPdfDllName, StringComparison.OrdinalIgnoreCase))
                return IntPtr.Zero;

            var fileNames = GetNativeFileNames();
            var runtimeIdentifier = SystemManager.GetRuntimeIdentifier();
            var architecture = SystemManager.GetPlatformName();

            foreach (var baseDirectory in GetBaseDirectories(assembly))
            {
                if (string.IsNullOrEmpty(baseDirectory))
                    continue;

                foreach (var fileName in fileNames)
                {
                    // Preferred RID-specific layout: runtimes/<rid>/native/<file>
                    var runtimeNativePath = Path.Combine(baseDirectory, "runtimes", runtimeIdentifier, "native", fileName);
                    if (TryLoad(runtimeNativePath, out var handle))
                        return handle;

                    // Legacy layout: <arch>/<file> (e.g. x64/qpdf30.dll)
                    var legacyPath = Path.Combine(baseDirectory, architecture, fileName);
                    if (TryLoad(legacyPath, out handle))
                        return handle;
                }
            }

            // Fall back to the default .NET resolution (including automatic runtimes/<rid>/native
            // resolution via deps.json) by returning IntPtr.Zero.
            Logger.LogInformation($"Could not resolve native library '{libraryName}' from the runtimes or legacy folders, falling back to default resolution");
            return IntPtr.Zero;
        }
        #endregion

        #region Helpers
        private static bool TryLoad(string path, out IntPtr handle)
        {
            handle = IntPtr.Zero;

            if (!File.Exists(path))
                return false;

            if (NativeLibrary.TryLoad(path, out handle))
            {
                Logger.LogInformation($"Successfully loaded native library from '{path}'");
                return true;
            }

            Logger.LogError($"Found but failed to load native library from '{path}'");
            return false;
        }

        private static string[] GetBaseDirectories(Assembly assembly)
        {
            return
            [
                AppContext.BaseDirectory,
                Path.GetDirectoryName(assembly.Location) ?? string.Empty
            ];
        }

        private static string[] GetNativeFileNames()
        {
            // qpdf uses versioned sonames on Unix (e.g. libqpdf.so.30, libqpdf.30.dylib) while on
            // Windows the version is part of the file name (qpdf30.dll). Several candidate names are
            // probed so the loader keeps working across qpdf releases.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return [Constants.QPdfDllName + ".dll"];

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return
                [
                    "libqpdf.30.dylib",
                    "lib" + Constants.QPdfDllName + ".dylib",
                    "libqpdf.dylib"
                ];

            return
            [
                "libqpdf.so.30",
                "lib" + Constants.QPdfDllName + ".so",
                "libqpdf.so"
            ];
        }
        #endregion

        #region Native methods
        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdf_get_qpdf_version))]
        [return: MarshalAs(UnmanagedType.LPTStr)]
        internal static extern string qpdf_get_qpdf_version();

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_create))]
        internal static extern IntPtr qpdflogger_create();

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_cleanup))]
        internal static extern void qpdflogger_cleanup(IntPtr loggerHandle);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_init))]
        internal static extern IntPtr qpdfjob_init();

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_cleanup))]
        internal static extern void qpdfjob_cleanup(IntPtr jobHandle);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_set_logger))]
        internal static extern void qpdfjob_set_logger(IntPtr jobHandle, IntPtr loggerHandle);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_info))]
        internal static extern void qpdflogger_set_info(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_warn))]
        internal static extern void qpdflogger_set_warn(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_error))]
        internal static extern void qpdflogger_set_error(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdflogger_set_save))]
        internal static extern void qpdflogger_set_save(IntPtr loggerHandle, qpdf_log_dest_e destination, IntPtr callBackHandler, IntPtr udata);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_initialize_from_json))]
        internal static extern int qpdfjob_initialize_from_json(IntPtr jobHandle, IntPtr json);

        [DllImport(Constants.QPdfDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(qpdfjob_run))]
        internal static extern int qpdfjob_run(IntPtr jobHandle);
        #endregion
    }
}
#endif