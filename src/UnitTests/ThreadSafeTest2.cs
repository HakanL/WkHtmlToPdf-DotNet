using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WkHtmlToPdfDotNet.UnitTests
{
    [TestClass]
    public class ThreadSafeTest2 : TestBase
    {
        [TestMethod]
        [DataRow("TestPdf-1")]
        [DataRow("TestPdf-2")]
        [DataRow("TestPdf-3")]
        [DataRow("TestPdf-4")]
        [DataRow("TestPdf-5")]
        [DataRow("TestPdf-6")]
        [DataRow("TestPdf-7")]
        [DataRow("TestPdf-8")]
        [DataRow("TestPdf-9")]
        [DataRow("TestPdf-10")]
        public void ConvertToPdf(string documentTitle)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Grayscale,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10 },
                DocumentTitle = documentTitle,
            },
                Objects = {
                new ObjectSettings() {
                    Page = "http://www.color-hex.com/"
                }
            }
            };

            Converter.Convert(doc);
        }
    }
}
