using System;
using WkHtmlToPdfDotNet.EventDefinitions;

namespace WkHtmlToPdfDotNet.Contracts
{
    public interface IConverter : IDisposable
    {
        /// <summary>
        ///  Converts document based on given settings
        /// </summary>
        /// <param name="document">Document to convert</param>
        /// <returns>Returns converted document in bytes</returns>
        byte[] Convert(IDocument document);

        event EventHandler<PhaseChangedArgs> PhaseChanged;

        event EventHandler<ProgressChangedArgs> ProgressChanged;

        event EventHandler<FinishedArgs> Finished;

        event EventHandler<ErrorArgs> Error;

        event EventHandler<WarningArgs> Warning;

        ITools Tools { get; }
    }
}
