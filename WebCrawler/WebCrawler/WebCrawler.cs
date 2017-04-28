using Newtonsoft.Json;
using System;
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
        HashSet<string> _visitedPages = new HashSet<string>();
        HashSet<string> _skipped = new HashSet<string>();
        HashSet<string> _badLinks = new HashSet<string>();

        public WebCrawler(IInternet internet)
        {
            _internet = internet;
        }
        public void CrawlInternet(IWebPage webPage)
        {
            if (!_internet.IsValidPage(webPage.Address))
                _badLinks.Add(webPage.Address);
            else if (_visitedPages.Contains(webPage.Address))
                _skipped.Add(webPage.Address);
            else
            {
                _visitedPages.Add(webPage.Address);
                foreach (var linkAddress in webPage.Links)
                {
                    if (_internet.IsValidPage(linkAddress))
                        CrawlInternet(_internet[linkAddress]);
                    else
                        _badLinks.Add(linkAddress);
                }
            }
        }

        public string GetSuccessfullyVisitsJson()
        {
            return JsonConvert.SerializeObject(_visitedPages);
        }
        public string GetSkippedJson()
        {
            return JsonConvert.SerializeObject(_skipped);
        }
        public string GetErrorsJson()
        {
            return JsonConvert.SerializeObject(_badLinks);
        }
    }
}
