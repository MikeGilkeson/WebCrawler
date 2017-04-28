using System;

namespace WebCrawler
{
    public class InternetParseException : Exception
    {
        public InternetParseException(Exception innerException) : base("The Internet JSON is invalid", innerException) { }
    }
}
