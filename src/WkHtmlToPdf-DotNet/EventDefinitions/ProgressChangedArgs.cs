using System;
using WkHtmlToPdfDotNet.Contracts;

namespace WkHtmlToPdfDotNet.EventDefinitions
{
    public class ProgressChangedArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public string Description { get; set; }
    }
}
