using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using WebPageMonitor.Core.Enums;

namespace WebPageMonitor.Core.Entities
{
    public class ChangeLog
    {
        public int Id { get; set; }
        public string DiffContent { get; set; } = string.Empty;
        public WebSiteType SiteType { get; set; }
        public DateTime ChangeDate { get; set; }
        public int PageVersionId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public PageVersion? PageVersion { get; set; }
    }

}

