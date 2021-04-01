using System;
using System.Runtime.InteropServices;

namespace WkHtmlToPdfDotNet
{
    internal class WkHtmlModuleLinux86 : IWkHtmlModule
    {
        public WkHtmlModuleLinux86()
        {
            // Test
            Version();
        }

        public int ExtendedQT() => wkhtmltopdf_extended_qt();

        public IntPtr Version() => wkhtmltopdf_version();

        public int Init(int useGraphics) => wkhtmltopdf_init(useGraphics);

        public int DeInit() => wkhtmltopdf_deinit();

        public IntPtr CreateGlobalSettings() => wkhtmltopdf_create_global_settings();

        public int SetGlobalSetting(IntPtr settings, [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string name, [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string value) => wkhtmltopdf_set_global_setting(settings, name, value);

        public int GetGlobalSetting(IntPtr settings, [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string name, IntPtr value, int valueSize) => wkhtmltopdf_get_global_setting(settings, name, value, valueSize);

        public int DestroyGlobalSettings(IntPtr settings) => wkhtmltopdf_destroy_global_settings(settings);

        public IntPtr CreateObjectSettings() => wkhtmltopdf_create_object_settings();

        public int SetObjectSetting(IntPtr settings, [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string name, [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string value) => wkhtmltopdf_set_object_setting(settings, name, value);

        public int GetObjectSetting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name, IntPtr value, int valueSize) => wkhtmltopdf_get_object_setting(settings, name, value, valueSize);

        public int DestroyObjectSettings(IntPtr settings) => wkhtmltopdf_destroy_object_settings(settings);

        public IntPtr CreateConverter(IntPtr globalSettings) => wkhtmltopdf_create_converter(globalSettings);

        public void AddObject(IntPtr converter,
            IntPtr objectSettings,
            byte[] data) => wkhtmltopdf_add_object(converter, objectSettings, data);

        public void AddObject(IntPtr converter,
            IntPtr objectSettings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string data) => wkhtmltopdf_add_object(converter, objectSettings, data);

        public bool Convert(IntPtr converter) => wkhtmltopdf_convert(converter);

        public void DestroyConverter(IntPtr converter) => wkhtmltopdf_destroy_converter(converter);

        public int GetOutput(IntPtr converter, out IntPtr data) => wkhtmltopdf_get_output(converter, out data);

        public int SetPhaseChangedCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback) => wkhtmltopdf_set_phase_changed_callback(converter, callback);

        public int SetProgressChangedCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback) => wkhtmltopdf_set_progress_changed_callback(converter, callback);

        public int SetFinishedCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback) => wkhtmltopdf_set_finished_callback(converter, callback);

        public int SetWarningCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback) => wkhtmltopdf_set_warning_callback(converter, callback);

        public int SetErrorCallback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback) => wkhtmltopdf_set_error_callback(converter, callback);

        public int PhaseCount(IntPtr converter) => wkhtmltopdf_phase_count(converter);

        public int CurrentPhase(IntPtr converter) => wkhtmltopdf_current_phase(converter);

        public IntPtr PhaseDescription(IntPtr converter, int phase) => wkhtmltopdf_phase_description(converter, phase);

        public IntPtr ProgressString(IntPtr converter) => wkhtmltopdf_progress_string(converter);

        public int HttpErrorCode(IntPtr converter) => wkhtmltopdf_http_error_code(converter);

        public const string DLLNAME = "runtimes/linux-x86/native/libwkhtmltox";

        const CharSet CHARSET = CharSet.Unicode;

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_extended_qt();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_version();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_init(int useGraphics);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_deinit();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_create_global_settings();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_global_setting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string value);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_get_global_setting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            IntPtr value, int valueSize);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_destroy_global_settings(IntPtr settings);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_create_object_settings();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_object_setting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string value);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_get_object_setting(IntPtr settings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)]
            string name,
            IntPtr value, int valueSize);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_destroy_object_settings(IntPtr settings);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_create_converter(IntPtr globalSettings);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_add_object(IntPtr converter,
            IntPtr objectSettings,
            byte[] data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_add_object(IntPtr converter,
            IntPtr objectSettings,
            [MarshalAs((short)CustomUnmanagedType.LPUTF8Str)] string data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern bool wkhtmltopdf_convert(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_destroy_converter(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_get_output(IntPtr converter, out IntPtr data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_phase_count(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_current_phase(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_phase_description(IntPtr converter, int phase);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_progress_string(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_http_error_code(IntPtr converter);
    }
}
