using Newtonsoft.Json.Linq;
using System;
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
            try
            {
                var jsonObject = JObject.Parse(internetJson);
                foreach (var token in jsonObject["pages"].Children())   // get all of the pages in the JSON
                {
                    var page = token.ToObject<WebPage>();               // convert the token to a WebPage
                    if (internet._webPagesDictionary.Count == 0)        // store the first one, that the one we're going to start the crawl with.
                        internet.FirstPage = page;
                    internet._webPagesDictionary.TryAdd(page.Address, page);    // If the address already exists should we merge the Links? Or treat as an error?
                }
            }
            catch (Exception exception)
            {
                throw new InternetParseException(exception);        // Should give a more accurate error, for time reasons keep it simple
            }
            return internet;
        }
    }
}
