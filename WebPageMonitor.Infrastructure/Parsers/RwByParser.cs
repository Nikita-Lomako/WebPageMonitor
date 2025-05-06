using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Parsers;

namespace WebPageMonitor.Infrastructure.Parsers
{
    public class RwByParser : IWebPageParser
    {
        private readonly HttpClient _httpClient;

        public RwByParser(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPageContentAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            // возможно, нужна дополнительная обработка HTML
            return content;
        }
    }
}
