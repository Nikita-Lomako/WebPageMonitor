using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Enums;
using WebPageMonitor.Infrastructure.Data;

namespace WebPageMonitor.Infrastructure.Repositories
{
    public class EfChangeLogRepository : IChangeLogRepository
    {
        private readonly AppDbContext _context;

        public EfChangeLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChangeLog>> GetBySiteTypeAsync(WebSiteType siteType) =>
            await _context.ChangeLogs
                .Where(c => c.SiteType == siteType)
                .ToListAsync();

        public async Task AddAsync(ChangeLog log)
        {
            _context.ChangeLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
