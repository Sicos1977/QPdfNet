//
// SystemManager.cs
//
// Project URL: https://github.com/AndreyAkinshin/InteropDotNet
//
// Copyright (c) 2014 Andrey Akinshin
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

namespace QPdfNet.InteropDotNet
{
    #region Internal enum OperatingSystem
    internal enum OperatingSystem
    {
        Windows,

        Unix,

        MacOSX,

        Unknown
    }
    #endregion

    internal static class SystemManager
    {
        #region GetPlatformName
        public static string GetPlatformName()
        {
            return IntPtr.Size == sizeof(int) ? "x86" : "x64";
        }
        #endregion

        #region GetArchitectureName
        /// <summary>
        ///     Returns the processor architecture part of a runtime identifier (e.g. <c>x64</c>, <c>x86</c>,
        ///     <c>arm64</c> or <c>arm</c>).
        /// </summary>
        public static string GetArchitectureName()
        {
            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X86:
                    return "x86";
                case Architecture.X64:
                    return "x64";
                case Architecture.Arm:
                    return "arm";
                case Architecture.Arm64:
                    return "arm64";
                default:
                    return IntPtr.Size == sizeof(int) ? "x86" : "x64";
            }
        }
        #endregion

        #region GetRuntimeIdentifier
        /// <summary>
        ///     Returns the RID (runtime identifier) for the current operating system and process
        ///     architecture, e.g. <c>win-x64</c>, <c>win-x86</c>, <c>linux-x64</c>, <c>linux-arm64</c>,
        ///     <c>osx-x64</c> or <c>osx-arm64</c>. This is used to resolve native assets from the
        ///     <c>runtimes/&lt;rid&gt;/native</c> folder layout.
        /// </summary>
        public static string GetRuntimeIdentifier()
        {
            var architecture = GetArchitectureName();

            switch (GetOperatingSystem())
            {
                case OperatingSystem.Windows:
                    return "win-" + architecture;
                case OperatingSystem.Unix:
                    return "linux-" + architecture;
                case OperatingSystem.MacOSX:
                    return "osx-" + architecture;
                default:
                    return architecture;
            }
        }
        #endregion

        #region GetOperatingSystem
        public static OperatingSystem GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OperatingSystem.Windows;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OperatingSystem.Unix;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OperatingSystem.MacOSX;

            return OperatingSystem.Unknown;
        }
        #endregion
    }
}