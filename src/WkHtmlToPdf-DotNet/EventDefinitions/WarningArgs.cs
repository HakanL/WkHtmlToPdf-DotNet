using System;
using WkHtmlToPdfDotNet.Contracts;

namespace WkHtmlToPdfDotNet.EventDefinitions
{
    public class WarningArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public string Message { get; set; }
    }
}
