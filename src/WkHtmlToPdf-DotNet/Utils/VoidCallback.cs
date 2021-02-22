using System;
using System.Runtime.InteropServices;

namespace WkHtmlToPdfDotNet
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VoidCallback(IntPtr converter);
}
