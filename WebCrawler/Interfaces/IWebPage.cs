namespace WebCrawler.Interfaces
{
    public interface IWebPage
    {
        string Address { get; }
        string[] Links { get; }
    }
}
