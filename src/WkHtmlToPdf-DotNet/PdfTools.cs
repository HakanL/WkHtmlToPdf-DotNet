using WkHtmlToPdfDotNet.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Concurrent;

namespace WkHtmlToPdfDotNet
{
    public sealed class PdfTools : ITools
    {
        private IWkHtmlModule module;
        // Used to maintain a reference to delegates to prevent them being garbage collected
        private readonly ConcurrentDictionary<IntPtr, List<object>> delegates = new ConcurrentDictionary<IntPtr, List<object>>();

        public bool IsLoaded { get; private set; }

        public PdfTools()
        {
            IsLoaded = false;
        }

        public void Load()
        {
            if (IsLoaded)
            {
                return;
            }

            this.module = ModuleFactory.GetModule();

            if (this.module.Init(0) == 1)
            {
                IsLoaded = true;
            }
        }

        public bool ExtendedQt()
        {
            return this.module.ExtendedQT() == 1;
        }

        public string GetLibraryVersion()
        {
            return Marshal.PtrToStringAnsi(this.module.Version());
        }

        public IntPtr CreateGlobalSettings()
        {
            return this.module.CreateGlobalSettings();
        }

        public int SetGlobalSetting(IntPtr settings, string name, string value)
        {
            return this.module.SetGlobalSetting(settings, name, value);
        }

        public string GetGlobalSetting(IntPtr settings, string name)
        {
            // Default const char * size is 2048 bytes
            byte[] buffer = new byte[2048];

            int size = Marshal.SizeOf(buffer[0]) * buffer.Length;
            IntPtr pnt = Marshal.AllocHGlobal(size);

            try
            {
                // Copy the array to unmanaged memory
                Marshal.Copy(buffer, 0, pnt, buffer.Length);

                this.module.GetGlobalSetting(settings, name, pnt, size);

                // And copy back the result from unmanaged memory
                Marshal.Copy(pnt, buffer, 0, size);
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }

            return GetString(buffer);
        }

        public void DestroyGlobalSetting(IntPtr settings)
        {
            this.module.DestroyGlobalSettings(settings);
        }

        public IntPtr CreateObjectSettings()
        {
            return this.module.CreateObjectSettings();
        }

        public int SetObjectSetting(IntPtr settings, string name, string value)
        {
            return this.module.SetObjectSetting(settings, name, value);
        }

        public string GetObjectSetting(IntPtr settings, string name)
        {
            // Default const char * size is 2048 bytes
            byte[] buffer = new byte[2048];

            int size = Marshal.SizeOf(buffer[0]) * buffer.Length;
            IntPtr pnt = Marshal.AllocHGlobal(size);

            try
            {
                // Copy the array to unmanaged memory
                Marshal.Copy(buffer, 0, pnt, buffer.Length);

                this.module.GetObjectSetting(settings, name, pnt, size);

                // And copy back the result from unmanaged memory
                Marshal.Copy(pnt, buffer, 0, size);
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }

            return GetString(buffer);
        }

        public void DestroyObjectSetting(IntPtr settings)
        {
            this.module.DestroyObjectSettings(settings);
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            return this.module.CreateConverter(globalSettings);
        }

        public void AddObject(IntPtr converter, IntPtr objectSettings, byte[] data)
        {
            this.module.AddObject(converter, objectSettings, data);
        }

        public void AddObject(IntPtr converter, IntPtr objectSettings, string data)
        {
            this.module.AddObject(converter, objectSettings, data);
        }

        public bool DoConversion(IntPtr converter)
        {
            return this.module.Convert(converter);
        }

        public void DestroyConverter(IntPtr converter)
        {
            // Delete delegates
            this.delegates.TryRemove(converter, out var l);
            l?.Clear();

            this.module.DestroyConverter(converter);
        }

        public byte[] GetConversionResult(IntPtr converter)
        {
            IntPtr resultPointer;

            int length = this.module.GetOutput(converter, out resultPointer);
            var result = new byte[length];
            Marshal.Copy(resultPointer, result, 0, length);

            return result;
        }

        private void AddCallback(IntPtr converter, object callback)
        {
            if (!this.delegates.TryGetValue(converter, out var list))
            {
                list = new List<object>();
                this.delegates.TryAdd(converter, list);

                this.delegates.TryGetValue(converter, out list);
            }

            list.Add(callback);
        }

        public int SetPhaseChangedCallback(IntPtr converter, VoidCallback callback)
        {
            AddCallback(converter, callback);

            return this.module.SetPhaseChangedCallback(converter, callback);
        }

        public int SetProgressChangedCallback(IntPtr converter, VoidCallback callback)
        {
            AddCallback(converter, callback);

            return this.module.SetProgressChangedCallback(converter, callback);
        }

        public int SetFinishedCallback(IntPtr converter, IntCallback callback)
        {
            AddCallback(converter, callback);

            return this.module.SetFinishedCallback(converter, callback);
        }

        public int SetWarningCallback(IntPtr converter, StringCallback callback)
        {
            AddCallback(converter, callback);

            return this.module.SetWarningCallback(converter, callback);
        }

        public int SetErrorCallback(IntPtr converter, StringCallback callback)
        {
            AddCallback(converter, callback);

            return this.module.SetErrorCallback(converter, callback);
        }

        public int GetPhaseCount(IntPtr converter)
        {
            return this.module.PhaseCount(converter);
        }

        public int GetCurrentPhase(IntPtr converter)
        {
            return this.module.CurrentPhase(converter);
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            return Marshal.PtrToStringAnsi(this.module.PhaseDescription(converter, phase));
        }

        public string GetProgressString(IntPtr converter)
        {
            return Marshal.PtrToStringAnsi(this.module.ProgressString(converter));
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // Free unmanaged resources (unmanaged objects) and override a finalizer below.
                this.module?.DeInit();
                // TODO: set large fields to null.

                this.disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~PdfTools()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

        private string GetString(byte[] buffer)
        {
            int nullPos = Array.IndexOf<byte>(buffer, 0);
            if (nullPos == -1)
                nullPos = buffer.Length;

            return Encoding.UTF8.GetString(buffer, 0, nullPos);
        }
    }
}
