using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Interfaces;

namespace WebCrawler
{
    public class WebPage : IWebPage
    {
        public WebPage(string address, string [] links)
        {
            Address = address;
            Links = links;
        }
        public string Address { private set; get; }
        public string [] Links { private set; get; }
    }
}
