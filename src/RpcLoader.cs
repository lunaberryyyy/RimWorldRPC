using System;
using System.IO;
using System.Runtime.InteropServices;
using Verse;

namespace RimRPC
{
    internal static class RpcLoader
    {
        public static void LoadNativeLibrary()
        {
            try
            {
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
                string modRoot = Path.Combine(GenFilePaths.ModsFolderPath, "RimRPC");
                string libPath = null;

                if (isWindows)
                {
                    libPath = Path.Combine(modRoot, "Libs", "win-x64", "0discord-rpc.dll");
                }
                else if (isLinux)
                {
                    libPath = Path.Combine(modRoot, "Libs", "linux-x64", "lib0discord-rpc.so");
                }

                if (string.IsNullOrEmpty(libPath) || !File.Exists(libPath))
                {
                    Log.Error("[RimRPC] Native library not found: " + libPath);
                    return;
                }

                IntPtr handle = LoadLibraryOS(libPath);

                if (handle == IntPtr.Zero)
                {
                    Log.Error("[RimRPC] Failed to load native library: " + libPath);
                }
                else
                {
                    Log.Message("[RimRPC] Successfully loaded native library.");
                }
            }
            catch (Exception e)
            {
                Log.Error("[RimRPC] Exception while loading native library: " + e);
            }
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("libdl.so.2", EntryPoint = "dlopen")]
        private static extern IntPtr dlopen(string fileName, int flags);

        private static IntPtr LoadLibraryOS(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return LoadLibrary(path);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return dlopen(path, 2);

            return IntPtr.Zero;
        }
    }
}