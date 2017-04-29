namespace WebCrawler.Interfaces
{
    public interface IWebAddressCollection
    {
        bool TryAdd(string address);
        bool Contains(string address);
        void Clear();
        string GetJson();
    }
}
