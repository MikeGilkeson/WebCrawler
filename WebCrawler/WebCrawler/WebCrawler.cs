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
            try
            {
                if (!_internet.IsValidPage(webPage.Address))                // This case would only be hit for the initial call, all other would get caught in the Parallel.ForEach loop. Think of a better way to handle this case.
                    _badLinks.TryAdd(webPage.Address, null);
                else if (_visitedPages.ContainsKey(webPage.Address))
                    _skipped.TryAdd(webPage.Address, null);
                else
                {
                    _visitedPages.TryAdd(webPage.Address, null);
                    Parallel.ForEach<string>(webPage.Links, linkAddress =>  // this isn't the best way to achieve parallel processing in this case (it will only wait for the current links to finish instead of all).
                    {                                                   // It would be better to use a thread pool and wait for all threads to finish.
                        if (_internet.IsValidPage(linkAddress))             // For time reasons, I'll leave it for now.
                            CrawlInternet(_internet[linkAddress]);
                        else
                            _badLinks.TryAdd(linkAddress, null);
                    });
                }
            }
            catch (Exception exception)
            {
                throw new InternetParseException(exception);        // Should give a more accurate error. If exception happens in the ForEach maybe continue on to next. For time reasons keep it simple.
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
