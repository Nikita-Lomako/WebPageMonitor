using System.Net.Http.Json;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Enums;

namespace WebPageMonitor.Client.Services
{
    public class WatchedPageService
    {
        private readonly HttpClient _http;

        public WatchedPageService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<WatchedPage>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<WatchedPage>>("api/watchedpages") ?? new();
        }

        public async Task StartTrackingAsync(int pageId, string email)
        {
            var payload = new { pageId, email };
            await _http.PostAsJsonAsync("api/watchedpages/start", payload);
        }

        public async Task StopTrackingAsync(int pageId)
        {
            await _http.PostAsJsonAsync("api/watchedpages/stop", new { pageId });
        }

        public async Task CheckUpdatesAsync(WatchedPage page)
        {
            try
            {
                if (page.Type == WebSiteType.Gismeteo)
                    await _http.PostAsync($"api/weather/{page.Id}/check-updates", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении: {ex.Message}");
            }
        }
    }
}
