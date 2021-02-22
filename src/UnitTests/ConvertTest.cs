using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace WkHtmlToPdfDotNet.UnitTests
{
    [TestClass]
    public class ConvertTest
    {
        private static SynchronizedConverter Converter;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Converter = new SynchronizedConverter(new PdfTools());
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Converter.Dispose();
        }

        [TestMethod]
        public void ConvertToPdf()
        {
            byte[] pdf = GetPdfWithTableOfContents();

            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 20000);
        }

        [TestMethod]
        public void RepeatTableOfContents()
        {
            int? size = null;

            for (int i = 0; i < 3; i++)
            {
                byte[] pdf = GetPdfWithTableOfContents();

                size ??= pdf.Length;

                Assert.IsNotNull(pdf);

                WriteToPdfFile(pdf);

                Assert.IsTrue(pdf.Length > 20000);
                Assert.AreEqual(size.Value, pdf.Length);
            }
        }

        private void WriteToPdfFile(byte[] pdf, string fileName = "")
        {
            if (!Directory.Exists("Files"))
            {
                Directory.CreateDirectory("Files");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}.pdf";
            }

            using (var stream = new FileStream(Path.Combine("Files", fileName), FileMode.Create))
            {
                stream.Write(pdf, 0, pdf.Length);
            }
        }

        [TestMethod]
        public void NormalConvertToPdf()
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices iaculis. Ut et odio viverra, molestie lectus nec, venenatis turpis. Nulla quis euismod nisl. Duis scelerisque eros nec dui facilisis, sit amet porta odio varius. Praesent vitae sollicitudin leo. Sed vitae quam in massa eleifend porta. Aliquam pulvinar orci dapibus porta laoreet. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Sed commodo tortor eget dolor hendrerit dapibus.
                                        Vivamus lorem diam, vulputate at ultrices quis, tristique eu nunc. Sed bibendum hendrerit leo. Nulla nec risus turpis. Vivamus at tortor felis. Donec eget posuere libero. Pellentesque erat nunc, molestie eget gravida vitae, eleifend a eros. Integer in tortor sed elit aliquam vehicula eget a erat. Vivamus nisi augue, venenatis ut commodo vel, congue id neque. Curabitur convallis dictum semper. Nulla accumsan urna aliquet, mattis dolor molestie, fermentum metus. Quisque at nisi non augue tempor commodo et pretium orci.
                                        Quisque blandit libero ut laoreet venenatis. Morbi sit amet quam varius, euismod dui et, volutpat felis. Sed nec ante vel est convallis placerat. Morbi mollis pretium tempor. Aliquam luctus eu justo vitae tristique. Sed in elit et elit sagittis pharetra sed vitae velit. Proin eget mi facilisis, scelerisque justo in, ornare urna. Aenean auctor ante ex, eget mattis neque pretium id. Aliquam ut risus leo. Vivamus ullamcorper et mauris in vehicula. Maecenas tristique interdum tempus. Etiam mattis lorem eget odio faucibus, in rhoncus nisi ultrices. Etiam at convallis nibh. Suspendisse tincidunt velit arcu, a volutpat nulla euismod sed.
                                        Aliquam mollis placerat blandit. Morbi in nibh urna. Donec nisl enim, tristique id tincidunt sed, pharetra non mi. Morbi viverra arcu vulputate risus dignissim efficitur. Vivamus dolor eros, finibus et porttitor a, pellentesque a lectus. Integer pellentesque maximus velit sit amet sollicitudin. Nulla a elit eget augue pretium luctus quis eu metus. Aenean nec dui id nibh tempor dapibus. Pellentesque dignissim ullamcorper mauris, vitae pharetra turpis sodales sit amet. Etiam et bibendum neque.
                                        Nulla gravida sit amet velit eu aliquet. Etiam sit amet elit leo. Sed nec arcu tincidunt, placerat turpis quis, laoreet nulla. Aenean neque est, fringilla non nulla in, laoreet vehicula nunc. Etiam vel nisl sit amet lectus pellentesque eleifend. Etiam sed nisi dolor. Mauris quis tincidunt ex. Aliquam porta mattis tempor. Maecenas fringilla bibendum elementum. Vestibulum quis tempus libero, vitae cursus neque. Suspendisse lectus risus, lacinia consectetur enim quis, ullamcorper porta tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                        FooterSettings = { FontSize = 9, Right = "Page [page] of [toPage]" }
                    }
                }
            };

            byte[] pdf = Converter.Convert(doc);

            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 10000);
        }

        [TestMethod]
        public void ConvertToPdfWithImg()
        {
            var redFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", "red.jpg");
            var blueFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", "blue.jpg");

            var docRed = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = $"<img src=\"{redFile}\">",
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            var docBlue = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = $"<img src=\"{blueFile}\">",
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            byte[] pdfRed = Converter.Convert(docRed);
            byte[] pdfBlue = Converter.Convert(docBlue);

            CollectionAssert.AreNotEqual(pdfRed, pdfBlue);
        }

        private byte[] GetPdfWithTableOfContents(string templatePage = "template.html")
        {
            var globalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            };
            var templateSettings = new ObjectSettings
            {
                Page = templatePage
            };

            var tableOfContentSettings = new TableOfContentsSettings
            {
                IsTableOfContent = true
            };

            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,

                Objects = { tableOfContentSettings, templateSettings }
            };

            byte[] pdf = Converter.Convert(doc);

            return pdf;
        }
    }
}
