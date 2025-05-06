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
        public string DiffContent { get; set; } = string.Empty; // JSON различий
        public WebSiteType SiteType { get; set; } // Для фильтрации по типу
        public DateTime ChangeDate { get; set; }
        public int PageVersionId { get; set; }
        public PageVersion PageVersion { get; set; } = new PageVersion();
    }
}

