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
    public class EfWatchedPageRepository : IWatchedPageRepository
    {
        private readonly AppDbContext _context;

        public EfWatchedPageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WatchedPage?> GetByIdAsync(int id) =>
            await _context.WatchedPages
                .Include(w => w.Versions)
                .FirstOrDefaultAsync(w => w.Id == id);

        public async Task<IEnumerable<WatchedPage>> GetAllAsync() =>
            await _context.WatchedPages
                .Include(w => w.Versions)
                .ToListAsync();

        public async Task AddAsync(WatchedPage page)
        {
            _context.WatchedPages.Add(page);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WatchedPage page)
        {
            _context.WatchedPages.Update(page);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var page = await _context.WatchedPages.FindAsync(id);
            if (page != null)
            {
                _context.WatchedPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }
    }
}
