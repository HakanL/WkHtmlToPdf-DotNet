using System;
using System.Runtime.InteropServices;

namespace WkHtmlToPdfDotNet
{
    internal static class ModuleFactory
    {
        public static IWkHtmlModule GetModule()
        {
            try
            {
#if NETSTANDARD2_0
                // Windows allows us to probe for either 64 or 86 bit versions
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    {
#else
                    if (IntPtr.Size == 8) // Is64 bit Arch
                    {
#endif
                        return new WkHtmlModuleWin64();
                    }
                    else
                    {
                        return new WkHtmlModuleWin86();
                    }
#if NETSTANDARD2_0
                }
                else
                {
                    switch (RuntimeInformation.ProcessArchitecture)
                    {
                        case Architecture.X64:
                            return new WkHtmlModuleLinux64();
                        case Architecture.X86:
                            return new WkHtmlModuleLinux86();
                        case Architecture.Arm:
                        case Architecture.Arm64:
                            return new WkHtmlModuleLinuxArm64();
                        default: // Unreachable
                            return new WkHtmlModule();
                    }
                }
#endif
            }
            catch (Exception)
            {
            }

            // Also try to load it with the method that should use the deps file
            try
            {
                return new WkHtmlModule();
            }
            catch
            {
            }

            throw new NotSupportedException("Unable to load native library. The platform may be missing native dependencies (libjpeg62, etc). Or the current platform is not supported.");
        }
    }
}
