using Microsoft.AspNetCore.Mvc;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Infrastructure.Repositories;

namespace WebPageMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchedPagesController : ControllerBase
    {
        private readonly IWatchedPageRepository _watchedPageRepository;

        public WatchedPagesController(IWatchedPageRepository watchedPageRepository)
        {
            _watchedPageRepository = watchedPageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddWatchedPage([FromBody] WatchedPage page)
        {
            await _watchedPageRepository.AddAsync(page);
            return CreatedAtAction(nameof(GetWatchedPage), new { id = page.Id }, page);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWatchedPage(int id)
        {
            var page = await _watchedPageRepository.GetByIdAsync(id);
            if (page == null) return NotFound();
            return Ok(page);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pages = await _watchedPageRepository.GetAllAsync();
            return Ok(pages);
        }
    }
}
