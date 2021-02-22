namespace WkHtmlToPdfDotNet.Contracts
{
    public interface IObject : ISettings
    {
        byte[] GetContent();
    }
}
