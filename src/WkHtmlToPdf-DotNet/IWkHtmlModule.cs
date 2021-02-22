using System;
using System.Runtime.InteropServices;

namespace WkHtmlToPdfDotNet
{
    public interface IWkHtmlModule
    {
        int ExtendedQT();

        IntPtr Version();

        int Init(int useGraphics);

        int DeInit();

        IntPtr CreateGlobalSettings();

        int SetGlobalSetting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string value);

        int GetGlobalSetting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            IntPtr value,
            int valueSize);

        int DestroyGlobalSettings(IntPtr settings);

        IntPtr CreateObjectSettings();

        int SetObjectSetting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string value);

        int GetObjectSetting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            IntPtr value,
            int valueSize);

        int DestroyObjectSettings(IntPtr settings);

        IntPtr CreateConverter(IntPtr globalSettings);

        void AddObject(IntPtr converter,
            IntPtr objectSettings,
            byte[] data);

        void AddObject(IntPtr converter,
            IntPtr objectSettings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string data);

        bool Convert(IntPtr converter);

        void DestroyConverter(IntPtr converter);

        int GetOutput(IntPtr converter, out IntPtr data);

        int SetPhaseChangedCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

        int SetProgressChangedCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

        int SetFinishedCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

        int SetWarningCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        int SetErrorCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        int PhaseCount(IntPtr converter);

        int CurrentPhase(IntPtr converter);

        IntPtr PhaseDescription(IntPtr converter, int phase);

        IntPtr ProgressString(IntPtr converter);

        int HttpErrorCode(IntPtr converter);
    }
}
