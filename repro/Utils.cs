using System;
using System.Runtime.InteropServices;
using Emgu.CV;

namespace NormalizeTest4
{
    internal static class Utils
    {
        private static class Win32
        {
            // The OpenCV native dependency (x64/cvextern.dll) was compiled against ucrtbase, not
            // msvcrt. And... these C runtimes do not share the environment cache.
            // Hence, if we want OpenCV to see the environment variables we set here, we must use this runtime!
            private const string crt = "ucrtbase.dll";

            [DllImport(crt, EntryPoint = "_putenv_s", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
                SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            private static extern int _putenv_s([MarshalAs(UnmanagedType.LPStr)] string e, [MarshalAs(UnmanagedType.LPStr)] string v);
            public static void SetEnv(string variable, string value) => _putenv_s(variable, value);

            [DllImport(crt, EntryPoint = "getenv", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
                SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            private static extern IntPtr getenv([MarshalAs(UnmanagedType.LPStr)] string varName);
            public static string GetEnv(string varName) => Marshal.PtrToStringAnsi(getenv(varName));
        }

        private const string opencvOpenclDevice = "OPENCV_OPENCL_DEVICE";

        public static void EnableOpenCL(bool enable = true) => CvInvoke.UseOpenCL = enable;

        public static void SetOpenCLDeviceConfiguration(string configuration)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Win32.SetEnv(opencvOpenclDevice, configuration);
            else
                Environment.SetEnvironmentVariable(opencvOpenclDevice, configuration);
        }
    }
}
