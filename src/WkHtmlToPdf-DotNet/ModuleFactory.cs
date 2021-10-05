﻿using System;
using System.Runtime.InteropServices;

namespace WkHtmlToPdfDotNet
{
    internal static class ModuleFactory
    {
        public static IWkHtmlModule GetModule()
        {
            // First try the method used by the NuGet package (which uses the deps file)
            try
            {
                return new WkHtmlModule();
            }
            catch
            {
            }
#if NETSTANDARD2_0
            // Windows allows us to probe for either 64 or 86 bit versions
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    // Try 64-bit
                    VersionWin64();

                    return new WkHtmlModule();
                }
                catch
                {
                }

                try
                {
                    // Try 86-bit
                    VersionWin86();

                    return new WkHtmlModule();
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    return new WkHtmlModuleLinux64();
                }
                catch
                {
                }

                try
                {
                    return new WkHtmlModuleLinux86();
                }
                catch
                {
                }
            }
#endif
            throw new NotSupportedException("Unable to load native library. The platform may be missing native dependencies (libjpeg62, etc). Or the current platform is not supported.");
        }

        [DllImport(@"runtimes\win-x64\native\wkhtmltox", CharSet = CharSet.Unicode, EntryPoint = "wkhtmltopdf_version")]
        public static extern IntPtr VersionWin64();

        [DllImport(@"runtimes\win-x86\native\wkhtmltox", CharSet = CharSet.Unicode, EntryPoint = "wkhtmltopdf_version")]
        public static extern IntPtr VersionWin86();
    }
}
