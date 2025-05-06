using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using WebPageMonitor.Core.Enums;

namespace WebPageMonitor.Core.Entities
{
    public class WatchedPage
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public WebSiteType Type { get; set; } // Train или Weather
        public TimeSpan CheckInterval { get; set; }
        public DateTime LastChecked { get; set; }
        public List<PageVersion> Versions { get; set; } = new List<PageVersion>();
    }
}