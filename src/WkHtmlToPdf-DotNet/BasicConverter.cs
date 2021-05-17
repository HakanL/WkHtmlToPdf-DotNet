using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WkHtmlToPdfDotNet.Contracts;
using WkHtmlToPdfDotNet.EventDefinitions;
using System.Globalization;

namespace WkHtmlToPdfDotNet
{
    public class BasicConverter : IConverter
    {
        public ITools Tools { get; }

        public IDocument ProcessingDocument { get; private set; }

        public event EventHandler<PhaseChangedArgs> PhaseChanged;

        public event EventHandler<ProgressChangedArgs> ProgressChanged;

        public event EventHandler<FinishedArgs> Finished;

        public event EventHandler<ErrorArgs> Error;

        public event EventHandler<WarningArgs> Warning;

        public BasicConverter(ITools tools)
        {
            Tools = tools;
        }

        public virtual byte[] Convert(IDocument document)
        {
            if (!document.GetObjects().Any())
            {
                throw new ArgumentException("No objects is defined in document that was passed. At least one object must be defined.");
            }

            ProcessingDocument = document;

            byte[] result = null;

            if (!Tools.IsLoaded)
            {
                Tools.Load();
            }

            IntPtr converter = CreateConverter(document);

            // Register events
            Tools.SetPhaseChangedCallback(converter, OnPhaseChanged);
            Tools.SetProgressChangedCallback(converter, OnProgressChanged);
            Tools.SetFinishedCallback(converter, OnFinished);
            Tools.SetWarningCallback(converter, OnWarning);
            Tools.SetErrorCallback(converter, OnError);

            bool converted = Tools.DoConversion(converter);

            if (converted)
            {
                result = Tools.GetConversionResult(converter);
            }

            Tools.DestroyConverter(converter);
            ProcessingDocument = null;

            return result ?? Array.Empty<byte>();
        }

        private void OnPhaseChanged(IntPtr converter)
        {
            int currentPhase = Tools.GetCurrentPhase(converter);
            var eventArgs = new PhaseChangedArgs
            {
                Document = ProcessingDocument,
                PhaseCount = Tools.GetPhaseCount(converter),
                CurrentPhase = currentPhase,
                Description = Tools.GetPhaseDescription(converter, currentPhase)
            };

            PhaseChanged?.Invoke(this, eventArgs);
        }

        private void OnProgressChanged(IntPtr converter)
        {
            var eventArgs = new ProgressChangedArgs
            {
                Document = ProcessingDocument,
                Description = Tools.GetProgressString(converter)
            };

            ProgressChanged?.Invoke(this, eventArgs);
        }

        private void OnFinished(IntPtr converter, int success)
        {
            var eventArgs = new FinishedArgs
            {
                Document = ProcessingDocument,
                Success = success == 1
            };

            Finished?.Invoke(this, eventArgs);
        }

        private void OnError(IntPtr converter, string message)
        {
            var eventArgs = new ErrorArgs
            {
                Document = ProcessingDocument,
                Message = message
            };

            Error?.Invoke(this, eventArgs);
        }

        private void OnWarning(IntPtr converter, string message)
        {
            var eventArgs = new WarningArgs
            {
                Document = ProcessingDocument,
                Message = message
            };

            Warning?.Invoke(this, eventArgs);
        }

        private IntPtr CreateConverter(IDocument document)
        {
            IntPtr settings = Tools.CreateGlobalSettings();

            ApplyConfig(settings, document, true);

            IntPtr converter = Tools.CreateConverter(settings);

            foreach (var obj in document.GetObjects())
            {
                if (obj != null)
                {
                    settings = Tools.CreateObjectSettings();

                    ApplyConfig(settings, obj, false);

                    Tools.AddObject(converter, settings, obj.GetContent());
                }
            }

            return converter;
        }

        private void ApplyConfig(IntPtr config, ISettings settings, bool isGlobal)
        {
            if (settings == null)
            {
                return;
            }

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var props = settings.GetType().GetProperties(bindingFlags);

            foreach (var prop in props)
            {
                Attribute[] attrs = (Attribute[])prop.GetCustomAttributes();
                object propValue = prop.GetValue(settings);

                if (propValue == null)
                {
                    continue;
                }
                else if (attrs.Length > 0 && attrs[0] is WkHtmlAttribute)
                {
                    var attr = attrs[0] as WkHtmlAttribute;

                    Apply(config, attr.Name, propValue, isGlobal);
                }
                else if (propValue is ISettings)
                {
                    ApplyConfig(config, propValue as ISettings, isGlobal);
                }
            }
        }

        private void Apply(IntPtr config, string name, object value, bool isGlobal)
        {
            Func<IntPtr, string, string, int> applySetting;

            if (isGlobal)
            {
                applySetting = Tools.SetGlobalSetting;
            }
            else
            {
                applySetting = Tools.SetObjectSetting;
            }

            switch (value)
            {
                case bool boolValue:
                    applySetting(config, name, boolValue ? "true" : "false");
                    break;

                case double doubleValue:
                    applySetting(config, name, doubleValue.ToString("0.##", CultureInfo.InvariantCulture));
                    break;

                case object _ when typeof(Dictionary<string, string>).IsAssignableFrom(value.GetType()):
                    var dictionary = (Dictionary<string, string>)value;

                    int index = 0;
                    foreach (var pair in dictionary)
                    {
                        if (pair.Key == null || pair.Value == null)
                        {
                            continue;
                        }

                        //https://github.com/wkhtmltopdf/wkhtmltopdf/blob/c754e38b074a75a51327df36c4a53f8962020510/src/lib/reflect.hh#L192
                        applySetting(config, name + ".append", null);
                        applySetting(config, string.Format("{0}[{1}]", name, index), pair.Key + "\n" + pair.Value);

                        index++;
                    }
                    break;

                default:
                    applySetting(config, name, value.ToString());
                    break;
            };
        }

        public virtual void Dispose()
        {
            Tools.Dispose();
        }
    }
}
