using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Interfaces;

namespace WebCrawler
{
    public class WebCrawler
    {
        IInternet _internet;
        ConcurrentDictionary<string, object> _visitedPages = new ConcurrentDictionary<string, object>();
        ConcurrentDictionary<string, object> _skipped = new ConcurrentDictionary<string, object>();
        ConcurrentDictionary<string, object> _badLinks = new ConcurrentDictionary<string, object>();

        public WebCrawler(IInternet internet)
        {
            _internet = internet;
        }
        public void CrawlInternet(IWebPage webPage)
        {
            if (!_internet.IsValidPage(webPage.Address))
                _badLinks.TryAdd(webPage.Address, null);
            else if (_visitedPages.ContainsKey(webPage.Address))
                _skipped.TryAdd(webPage.Address, null);
            else
            {
                _visitedPages.TryAdd(webPage.Address, null);
                Parallel.ForEach<string>(webPage.Links, linkAddress =>
                    {
                        if (_internet.IsValidPage(linkAddress))
                            CrawlInternet(_internet[linkAddress]);
                        else
                            _badLinks.TryAdd(linkAddress, null);
                    });
            }
        }

        public string GetSuccessfullyVisitsJson()
        {
            return JsonConvert.SerializeObject(_visitedPages.Keys);
        }
        public string GetSkippedJson()
        {
            return JsonConvert.SerializeObject(_skipped.Keys);
        }
        public string GetErrorsJson()
        {
            return JsonConvert.SerializeObject(_badLinks.Keys);
        }
    }
}
