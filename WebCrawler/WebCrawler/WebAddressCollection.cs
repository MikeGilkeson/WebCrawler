using Newtonsoft.Json;
using System.Collections.Concurrent;
using WebCrawler.Interfaces;

namespace WebCrawler
{
    public class WebAddressCollection : IWebAddressCollection
    {
        ConcurrentDictionary<string, object> _addresses = new ConcurrentDictionary<string, object>();

        public bool TryAdd(string address)
        {
            return _addresses.TryAdd(address, null); ;
        }

        public bool Contains(string address)
        {
            return _addresses.ContainsKey(address);
        }

        public void Clear()
        {
            _addresses.Clear();
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(_addresses.Keys);
        }

    }
}
