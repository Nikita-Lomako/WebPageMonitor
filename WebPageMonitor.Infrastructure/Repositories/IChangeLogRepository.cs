using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Enums;

namespace WebPageMonitor.Infrastructure.Repositories
{
    public interface IChangeLogRepository
    {
        Task<IEnumerable<ChangeLog>> GetBySiteTypeAsync(WebSiteType siteType);
        Task<IEnumerable<ChangeLog>> GetAllAsync(); // Новый метод
        Task AddAsync(ChangeLog log);
        Task<ChangeLog?> GetByIdAsync(int id);
    }
}
