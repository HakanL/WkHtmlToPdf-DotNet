using System;
using System.Runtime.InteropServices;

namespace WkHtmlToPdfDotNet
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void StringCallback(IntPtr converter, [MarshalAs(UnmanagedType.LPStr)] string str);
}
