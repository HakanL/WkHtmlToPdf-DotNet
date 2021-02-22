using System;
using WkHtmlToPdfDotNet.Contracts;
using System.Text;

namespace WkHtmlToPdfDotNet
{
    public class ObjectSettings : IObject
    {
        /// <summary>
        /// The URL or path of the web page to convert, if "-" input is read from stdin. Default = ""
        /// </summary>
        [WkHtml("page")]
        public string Page { get; set; }

        /// <summary>
        /// Should external links in the HTML document be converted into external pdf links. Default = true
        /// </summary>
        [WkHtml("useExternalLinks")]
        public bool? UseExternalLinks { get; set; }

        /// <summary>
        /// Should internal links in the HTML document be converted into pdf references. Default = true
        /// </summary>
        [WkHtml("useLocalLinks")]
        public bool? UseLocalLinks { get; set; }

        /// <summary>
        /// Should we turn HTML forms into PDF forms. Default = false
        /// </summary>
        [WkHtml("produceForms")]
        public bool? ProduceForms { get; set; }

        /// <summary>
        /// Should the sections from this document be included in the outline and table of content. Default = false
        /// </summary>
        [WkHtml("includeInOutline")]
        public bool? IncludeInOutline { get; set; }

        /// <summary>
        /// Should we count the pages of this document, in the counter used for TOC, headers and footers. Default = false
        /// </summary>
        [WkHtml("pagesCount")]
        public bool? PagesCount { get; set; }

        public string HtmlContent { get; set; }

        public Encoding Encoding { get; set; }

        public WebSettings WebSettings { get; set; } = new WebSettings();

        public HeaderSettings HeaderSettings { get; set; } = new HeaderSettings();

        public FooterSettings FooterSettings { get; set; } = new FooterSettings();

        public LoadSettings LoadSettings { get; set; } = new LoadSettings();

        public byte[] GetContent()
            => HtmlContent != null ? (Encoding ?? Encoding.UTF8).GetBytes(HtmlContent) : Array.Empty<byte>();
    }
}
