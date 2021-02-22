using System;
using WkHtmlToPdfDotNet.Contracts;

namespace WkHtmlToPdfDotNet.EventDefinitions
{
    public class ErrorArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public string Message { get; set; }
    }
}
