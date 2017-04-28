using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Interfaces
{
    public interface IInternet
    {
        IWebPage[] Pages { get; }
        IWebPage this[string address]{ get; }
        bool IsValidPage(string address);
    }
}
