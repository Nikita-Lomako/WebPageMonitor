using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Entities;

namespace WebPageMonitor.Infrastructure.Repositories
{
    public interface IPageVersionRepository
    {
        Task<PageVersion?> GetByIdAsync(int id);
        Task<PageVersion?> GetLatestVersionAsync(int watchedPageId);
        Task<IEnumerable<PageVersion>> GetByWatchedPageIdAsync(int watchedPageId);
        Task AddAsync(PageVersion version);
        

    }
}
