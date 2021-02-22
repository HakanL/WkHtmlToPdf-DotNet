using System.Collections.Generic;
using WkHtmlToPdfDotNet.Contracts;

namespace WkHtmlToPdfDotNet
{
    public class HtmlToPdfDocument : IDocument
    {
        public List<ObjectSettings> Objects { get; } = new List<ObjectSettings>();

        public GlobalSettings GlobalSettings { get; set; } = new GlobalSettings();

        public IEnumerable<IObject> GetObjects() => Objects.ToArray();
    }
}
