using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageMonitor.Core.Parsers
{
    public interface IWebPageParser
    {
        Task<string> GetPageContentAsync(string url);
    }
}
