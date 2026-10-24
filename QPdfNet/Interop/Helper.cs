//
// Helper.cs
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

using System;
using System.IO;
using System.Runtime.InteropServices;
using QPdfNet.InteropDotNet;

namespace QPdfNet.Interop
{
    internal static class Helper
    {
        internal static void SetPath()
        {
            // Adding a directory to the PATH so that the operating system can locate the native
            // qpdf library and its dependent DLLs (vcruntime, msvcp, ...) is only relevant on
            // Windows. On Linux and macOS dependent shared libraries are resolved through the
            // system loader / rpath, so there is nothing to do here.
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            var baseDirectory =
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;

            // Preferred RID-specific layout (runtimes/win-x64/native) and the legacy x64/x86 folder.
            var nativeDirectories = new[]
            {
                Path.Combine(baseDirectory, Path.Combine("runtimes", Path.Combine(SystemManager.GetRuntimeIdentifier(), "native"))),
                Path.Combine(baseDirectory, Environment.Is64BitProcess ? "x64" : "x86")
            };

            foreach (var nativeDirectory in nativeDirectories)
            {
                if (!Directory.Exists(nativeDirectory))
                    continue;

                var path = Environment.GetEnvironmentVariable("path") ?? string.Empty;

                if (!path.Contains(nativeDirectory))
                    Environment.SetEnvironmentVariable("path", path + ";" + nativeDirectory);
            }
        }
    }
}