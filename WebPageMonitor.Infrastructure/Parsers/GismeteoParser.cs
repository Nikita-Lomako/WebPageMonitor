using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Parsers;
using HtmlAgilityPack;
using WebPageMonitor.Core.Models;

namespace WebPageMonitor.Infrastructure.Parsers
{
    public class GismeteoParser : IWebPageParser
    {
        private readonly HttpClient _httpClient;

        public GismeteoParser(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPageContentAsync(string url)
        {
            var weatherData = await GetWeatherDataAsync(url);
            return System.Text.Json.JsonSerializer.Serialize(weatherData);
        }

        public async Task<WeatherData> GetWeatherDataAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                var weatherData = new WeatherData
                {
                    ObservationTime = DateTime.UtcNow
                };

                // Get all time slots
                var timeNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'widget-row-datetime-time')]//div[contains(@class, 'row-item')]//span");
                if (timeNodes == null)
                    throw new Exception("Could not find time data");

                // Get temperature data
                var tempNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'widget-row-chart-temperature-air')]//div[contains(@class, 'value')]//temperature-value");
                if (tempNodes == null)
                    throw new Exception("Could not find temperature data");

                // Get wind data
                var windNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'widget-row-wind')]//div[contains(@class, 'row-item')]");
                if (windNodes == null)
                    throw new Exception("Could not find wind data");

                // Get humidity data
                var humidityNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'widget-row-humidity')]//div[contains(@class, 'row-item')]");
                if (humidityNodes == null)
                    throw new Exception("Could not find humidity data");

                // Get pressure data
                var pressureNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'widget-row-chart-pressure')]//div[contains(@class, 'value')]//pressure-value");
                if (pressureNodes == null)
                    throw new Exception("Could not find pressure data");

                // Get precipitation data
                var precipNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'widget-row-precipitation-bars')]//div[contains(@class, 'row-item')]//div[contains(@class, 'item-unit')]");
                if (precipNodes == null)
                    throw new Exception("Could not find precipitation data");

                // Combine all data into time slots
                for (int i = 0; i < timeNodes.Count; i++)
                {
                    var timeSlot = new TimeSlotData
                    {
                        Time = timeNodes[i].InnerText.Trim(),
                        Temperature = tempNodes[i].GetAttributeValue("value", "") + "°C",
                        WindSpeed = windNodes[i].SelectSingleNode(".//speed-value")?.InnerText?.Trim() + " м/с",
                        WindDirection = windNodes[i].SelectSingleNode(".//div[contains(@class, 'wind-direction')]")?.InnerText?.Trim() ?? "—",
                        Humidity = humidityNodes[i].InnerText.Trim() + "%",
                        Pressure = pressureNodes[i].GetAttributeValue("value", "") + " мм рт.ст.",
                        Precipitation = precipNodes[i].InnerText.Trim() + " мм"
                    };
                    weatherData.TimeSlots.Add(timeSlot);
                }

                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to fetch weather data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse weather data: {ex.Message}", ex);
            }
        }
    }
}

