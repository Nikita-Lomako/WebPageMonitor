using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Infrastructure.Data;

namespace WebPageMonitor.Infrastructure.Repositories
{
    public class EfPageVersionRepository : IPageVersionRepository
    {
        private readonly AppDbContext _context;

        public EfPageVersionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageVersion?> GetByIdAsync(int id) =>
            await _context.PageVersions
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<PageVersion?> GetLatestVersionAsync(int watchedPageId)
        {
            return await _context.PageVersions
                .Where(v => v.WatchedPageId == watchedPageId)
                .OrderByDescending(v => v.Timestamp)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PageVersion>> GetByWatchedPageIdAsync(int watchedPageId) =>
            await _context.PageVersions
                .Where(p => p.WatchedPageId == watchedPageId)
                .ToListAsync();

        public async Task AddAsync(PageVersion version)
        {
            _context.PageVersions.Add(version);
            await _context.SaveChangesAsync();
        }
    }
}
