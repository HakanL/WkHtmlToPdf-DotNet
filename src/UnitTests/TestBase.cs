using System;
using System.Collections.Generic;
using System.Text;

namespace WkHtmlToPdfDotNet.UnitTests
{
    public class TestBase
    {
        public static SynchronizedConverter Converter = new SynchronizedConverter(new PdfTools());
    }
}
