using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace WebPageMonitor.Core.Entities
{
    public class PageVersion
    {
        public int Id { get; set; }
        public string ContentHash { get; set; } = string.Empty; // SHA-256 хеш страницы
        public string Content { get; set; } = string.Empty; // JSON (например, список поездов)
        public DateTime Timestamp { get; set; }
        public int WatchedPageId { get; set; }
        public WatchedPage WatchedPage { get; set; } = new WatchedPage();
    }
}

