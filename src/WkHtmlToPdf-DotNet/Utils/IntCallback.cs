using System;
using System.Runtime.InteropServices;

namespace WkHtmlToPdfDotNet
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void IntCallback(IntPtr converter, int integer);
}
