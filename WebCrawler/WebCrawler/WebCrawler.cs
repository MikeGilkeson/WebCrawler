using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WebCrawler.Interfaces;

namespace WebCrawler
{
    public class WebCrawler
    {
        IInternet _internet;
        IWebAddressCollection _visitedPages;
        IWebAddressCollection _skippedPages;
        IWebAddressCollection _badPages;

        public WebCrawler(IInternet internet, IWebAddressCollection visitedPages, IWebAddressCollection skippedPages, IWebAddressCollection badPages)
        {
            if (internet == null)
                throw new NullReferenceException("internet is null");
            if (visitedPages == null)
                throw new NullReferenceException("visitedPages is null");
            if (skippedPages == null)
                throw new NullReferenceException("skippedPages is null");
            if (badPages == null)
                throw new NullReferenceException("badPages is null");
            _internet = internet;
            _visitedPages = visitedPages;
            _skippedPages = skippedPages;
            _badPages = badPages;
        }

        private void ClearCollections()
        {
            _visitedPages.Clear();
            _skippedPages.Clear();
            _badPages.Clear();
        }

        public void CrawlInternet(IWebPage webPage)
        {
            ClearCollections();
            if (!_internet.IsValidPage(webPage.Address))
            {
                _badPages.TryAdd(webPage.Address);
                return;
            }
            CrawlInternetInteral(webPage);
        }

        private void CrawlInternetInteral(IWebPage webPage)
        {
            try
            {
                if (_visitedPages.Contains(webPage.Address))
                    _skippedPages.TryAdd(webPage.Address);
                else
                {
                    _visitedPages.TryAdd(webPage.Address);
                    Parallel.ForEach<string>(webPage.Links, linkAddress =>  // this isn't the best way to achieve parallel processing in this case (it will only wait for the current links to finish instead of all).
                    {                                                       // It would be better to use a thread pool and wait for all threads to finish.
                        if (_internet.IsValidPage(linkAddress))             // For time reasons leave it
                            CrawlInternetInteral(_internet[linkAddress]);
                        else
                            _badPages.TryAdd(linkAddress);
                    });
                }
            }
            catch (CrawlException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CrawlException(exception);        // Should give a more accurate error. If exception happens in the ForEach maybe continue on to next. For time reasons keep it simple.
            }
        }
    }
}
