using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using WebCrawler.Interfaces;

namespace WebCrawler
{
    public class Internet : IInternet
    {
        private ConcurrentDictionary<string, IWebPage> _webPagesDictionary = new ConcurrentDictionary<string, IWebPage>();
        public IWebPage FirstPage { private set; get; }

        public IWebPage this[string address]
        {
            get
            {
                return _webPagesDictionary[address];
            }
        }

        public bool IsValidPage(string address)
        {
            return _webPagesDictionary.ContainsKey(address);
        }

        static public Internet Parse(string internetJson)
        {
            var internet = new Internet();
            var jsonObject = JObject.Parse(internetJson);
            foreach (var token in jsonObject["pages"].Children())
            {
                var page = token.ToObject<WebPage>();
                if (internet._webPagesDictionary.Count == 0)
                    internet.FirstPage = page;
                internet._webPagesDictionary.TryAdd(page.Address, page);
            }
            return internet;
        }
    }
}
