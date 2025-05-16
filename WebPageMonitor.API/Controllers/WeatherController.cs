using Microsoft.AspNetCore.Mvc;
using WebPageMonitor.Infrastructure.Services;
using System.Net;

namespace WebPageMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly GismeteoService _gismeteoService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(GismeteoService gismeteoService, ILogger<WeatherController> logger)
        {
            _gismeteoService = gismeteoService ?? throw new ArgumentNullException(nameof(gismeteoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("{watchedPageId}/check-updates")]
        public async Task<IActionResult> CheckUpdates(int watchedPageId)
        {
            try
            {
                if (watchedPageId <= 0)
                {
                    return BadRequest("Invalid watched page ID");
                }

                await _gismeteoService.CheckUpdatesAsync(watchedPageId);
                return Ok("Проверка завершена");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError(ex, "Weather page not found for watched page ID: {PageId}", watchedPageId);
                return NotFound("Страница погоды не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking updates for watched page ID: {PageId}", watchedPageId);
                return StatusCode(500, "Произошла ошибка при проверке обновлений");
            }
        }
    }
}
