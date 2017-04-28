
namespace WebCrawler.Interfaces
{
    public interface IInternet
    {
        IWebPage FirstPage { get; }
        IWebPage this[string address]{ get; }
        bool IsValidPage(string address);
    }
}
