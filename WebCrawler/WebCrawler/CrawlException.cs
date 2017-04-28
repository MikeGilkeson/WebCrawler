using System;

namespace WebCrawler
{
    public class CrawlException : Exception
    {
        public CrawlException(Exception innerException) : base("Failed to crawl the web", innerException) { }
    }
}
