using System.Collections.Generic;

namespace WkHtmlToPdfDotNet.Contracts
{
    public interface IDocument : ISettings
    {
        IEnumerable<IObject> GetObjects();
    }
}
