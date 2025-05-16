using System.Net.Http.Json;
using WebPageMonitor.Core.Entities;

namespace WebPageMonitor.Client.Services
{
    public class ChangeLogService
    {
        private readonly HttpClient _http;

        public ChangeLogService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ChangeLog>> GetAllAsync()
        {
            try
            {
                var response = await _http.GetFromJsonAsync<List<ChangeLog>>("api/changelog");
                return response ?? new List<ChangeLog>();
            }
            catch (HttpRequestException ex)
            {
                // Можно логировать ошибку
                Console.WriteLine($"Ошибка при получении ChangeLog: {ex.Message}");
                return new List<ChangeLog>(); // Возвращаем пустой список, чтобы компонент не падал
            }
        }


        public async Task<ChangeLog?> GetChangeByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ChangeLog>($"api/changelog/{id}");
        }
    }
}
