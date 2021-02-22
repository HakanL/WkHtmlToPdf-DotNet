using System;
using WkHtmlToPdfDotNet.Contracts;

namespace WkHtmlToPdfDotNet.EventDefinitions
{
    public class FinishedArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public bool Success { get; set; }
    }
}
