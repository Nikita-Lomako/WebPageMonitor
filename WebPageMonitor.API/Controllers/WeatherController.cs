using Microsoft.AspNetCore.Mvc;
using WebPageMonitor.Infrastructure.Services;

namespace WebPageMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly GismeteoService _gismeteoService;

        public WeatherController(GismeteoService gismeteoService)
        {
            _gismeteoService = gismeteoService;
        }

        [HttpPost("{watchedPageId}/check-updates")]
        public async Task<IActionResult> CheckUpdates(int watchedPageId)
        {
            try
            {
                await _gismeteoService.CheckUpdatesAsync(watchedPageId);
                return Ok("Проверка завершена");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
