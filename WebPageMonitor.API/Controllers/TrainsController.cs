using Microsoft.AspNetCore.Mvc;
using WebPageMonitor.Infrastructure.Services;

namespace WebPageMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainsController : ControllerBase
    {
        private readonly RwByService _rwByService;

        public TrainsController(RwByService rwByService)
        {
            _rwByService = rwByService;
        }

        [HttpPost("{watchedPageId}/check-updates")]
        public async Task<IActionResult> CheckUpdates(int watchedPageId)
        {
            try
            {
                await _rwByService.CheckUpdatesAsync(watchedPageId);
                return Ok("Проверка завершена");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
