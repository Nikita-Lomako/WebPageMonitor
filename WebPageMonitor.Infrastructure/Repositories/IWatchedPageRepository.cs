using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Entities;

namespace WebPageMonitor.Infrastructure.Repositories
{
    public interface IWatchedPageRepository
    {
        Task<WatchedPage?> GetByIdAsync(int id);
        Task<IEnumerable<WatchedPage>> GetAllAsync();
        Task AddAsync(WatchedPage page);
        Task UpdateAsync(WatchedPage page);
        Task DeleteAsync(int id);
    }
}
