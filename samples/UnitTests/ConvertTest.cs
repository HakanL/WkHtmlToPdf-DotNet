using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace UnitTests
{
    [TestClass]
    public class ConvertTest
    {
        private IConverter _converter;

        [TestInitialize]
        public void TestInitialize()
        {
            _converter = new SynchronizedConverter(new PdfTools());
        }

        [TestMethod]
        public void ConvertToPdfTest()
        {
            for (int i = 1; i < 3; i++)
            {
                var pdf = GetPdf();
                File.WriteAllBytes($"Test {i}.pdf", pdf);

                Assert.IsNotNull(pdf);
            }
        }

        private byte[] GetPdf(string templatePage = "")
        {
            if (string.IsNullOrEmpty(templatePage))
            {
                templatePage = "template.html";
            }
            var globalSettings = new GlobalSettings()
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            };
            var templateSettings = new ObjectSettings()
            {
                Page = templatePage,
            };

            var tableOfContentSettings = new TableOfContentsSettings()
            {
                IsTableOfContent = true
            };

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,

                Objects = { tableOfContentSettings, templateSettings }
            };

            byte[] pdf = _converter.Convert(doc);

            return pdf;
        }
    }

    public class TableOfContentsSettings : ObjectSettings
    {
        [WkHtml("isTableOfContent")]
        public bool IsTableOfContent { get; set; }

        public TableOfContentsSettings()
        {
            IsTableOfContent = true;
        }
    }
}
