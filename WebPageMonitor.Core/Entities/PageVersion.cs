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
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int WatchedPageId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public WatchedPage? WatchedPage { get; set; }
    }

}

