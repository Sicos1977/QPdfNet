using System;
using System.IO;

namespace QPdfNet.Interop
{
    internal static class Helper
    {
        internal static void SetPath()
        {
            var dllDirectory =
                Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                    Environment.Is64BitProcess ? "x64" : "x86");

            var path = Environment.GetEnvironmentVariable("path") ?? string.Empty;

            if (!path.Contains(dllDirectory))
                Environment.SetEnvironmentVariable("path", Environment.GetEnvironmentVariable("path") + ";" + dllDirectory);
        }
    }
}
