namespace WkHtmlToPdfDotNet
{
    public class TableOfContentsSettings : ObjectSettings
    {
        /// <summary>
        /// Should we create a table of contents
        /// </summary>
        [WkHtml("isTableOfContent")]
        public bool IsTableOfContent { get; set; }
        
        /// <summary>
        /// Name/Headline of the TOC
        /// </summary>
        [WkHtml("toc.captionText")]
        public string CaptionText { get; set; }

        /// <summary>
        /// Should we print dots between the name and the page number?
        /// </summary>
        [WkHtml("toc.useDottedLines")]
        public bool	UseDottedLines { get; set; }
        
        public TableOfContentsSettings()
        {
            IsTableOfContent = true;
            CaptionText = "Table of Contents";
            UseDottedLines = true;
        }
    }
}