using System.Net.Http.Json;
using WebPageMonitor.Core.Entities;

namespace WebPageMonitor.Client.Services
{
    public class PageVersionService
    {
        private readonly HttpClient _http;

        public PageVersionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PageVersion>> GetVersionsForPage(int pageId)
        {
            return await _http.GetFromJsonAsync<List<PageVersion>>($"api/pages/{pageId}/versions") ?? new();
        }
    }
}
