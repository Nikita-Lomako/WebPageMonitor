using Microsoft.AspNetCore.Mvc;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Enums;
using WebPageMonitor.Infrastructure.Repositories;

namespace WebPageMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChangeLogController : ControllerBase
    {
        private readonly IChangeLogRepository _changeLogRepository;

        public ChangeLogController(IChangeLogRepository changeLogRepository)
        {
            _changeLogRepository = changeLogRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChangeLog>>> GetAll()
        {
            var logs = await _changeLogRepository.GetAllAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChangeLog>> GetById(int id)
        {
            var log = await _changeLogRepository.GetByIdAsync(id);
            if (log == null)
                return NotFound();
            return Ok(log);
        }

        [HttpGet("by-site/{type}")]
        public async Task<ActionResult<IEnumerable<ChangeLog>>> GetBySite(WebSiteType type)
        {
            var all = await _changeLogRepository.GetAllAsync();
            var filtered = all.Where(c => c.SiteType == type);
            return Ok(filtered);
        }
    }
}
