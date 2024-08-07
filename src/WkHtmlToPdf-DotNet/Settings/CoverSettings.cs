using WkHtmlToPdfDotNet.Contracts;

namespace WkHtmlToPdfDotNet
{
    public class CoverSettings : ObjectSettings
    {
    /// <summary>
    /// Should we create a cover
    /// </summary>
    [WkHtml("isCover")]
    public bool IsCover { get; set; }

    public CoverSettings() {
      IsCover = true;
    }

  }
}
