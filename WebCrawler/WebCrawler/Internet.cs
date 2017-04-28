using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Interfaces;

namespace WebCrawler
{
    public class Internet : IInternet
    {
        private Dictionary<string, IWebPage> _webPagesDictionary = new Dictionary<string, IWebPage>();
        public IWebPage[] Pages { private set; get; }

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
            var parsed = JObject.Parse(internetJson);
            var webPages = new List<IWebPage>();
            foreach (var token in parsed["pages"].Children())
            {
                var page = token.ToObject<WebPage>();
                internet._webPagesDictionary.Add(page.Address, page);
                webPages.Add(token.ToObject<WebPage>());
            }
            internet.Pages = webPages.ToArray();
            return internet;
        }
    }
}
