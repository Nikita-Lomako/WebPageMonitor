using DiffPlex.DiffBuilder.Model;
using DiffPlex.DiffBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Parsers;
using WebPageMonitor.Infrastructure.Repositories;
using WebPageMonitor.Infrastructure.Parsers;
using WebPageMonitor.Core.Models;
using Microsoft.Extensions.Logging;

namespace WebPageMonitor.Infrastructure.Services
{
    public class GismeteoService
    {
        private readonly IWatchedPageRepository _watchedPageRepo;
        private readonly IPageVersionRepository _pageVersionRepo;
        private readonly IChangeLogRepository _changeLogRepo;
        private readonly GismeteoParser _parser;
        private readonly ILogger<GismeteoService> _logger;

        public GismeteoService(
            IWatchedPageRepository watchedPageRepo,
            IPageVersionRepository pageVersionRepo,
            IChangeLogRepository changeLogRepo,
            GismeteoParser parser,
            ILogger<GismeteoService> logger)
        {
            _watchedPageRepo = watchedPageRepo;
            _pageVersionRepo = pageVersionRepo;
            _changeLogRepo = changeLogRepo;
            _parser = parser;
            _logger = logger;
        }

        public async Task CheckUpdatesAsync(int watchedPageId)
        {
            try
            {
                var watchedPage = await _watchedPageRepo.GetByIdAsync(watchedPageId);
                if (watchedPage == null)
                {
                    _logger.LogError("Page not found with ID: {PageId}", watchedPageId);
                    throw new Exception("Page not found");
                }

                var newWeatherData = await _parser.GetWeatherDataAsync(watchedPage.Url);
                var newJson = JsonSerializer.Serialize(newWeatherData);

                var lastVersion = await _pageVersionRepo.GetLatestVersionAsync(watchedPageId);

                if (lastVersion == null)
                {
                    // First version
                    var newVersion = new PageVersion
                    {
                        Content = newJson,
                        Timestamp = DateTime.UtcNow,
                        WatchedPageId = watchedPageId
                    };
                    await _pageVersionRepo.AddAsync(newVersion);
                    _logger.LogInformation("Created first version for page {PageId}", watchedPageId);
                }
                else
                {
                    var oldWeather = JsonSerializer.Deserialize<WeatherData>(lastVersion.Content);
                    if (oldWeather == null)
                    {
                        _logger.LogError("Failed to deserialize old weather data for page {PageId}", watchedPageId);
                        throw new Exception("Failed to deserialize old weather data");
                    }

                    // Check if there are any actual changes
                    bool hasChanges = false;
                    if (oldWeather.TimeSlots.Count != newWeatherData.TimeSlots.Count)
                    {
                        hasChanges = true;
                    }
                    else
                    {
                        for (int i = 0; i < oldWeather.TimeSlots.Count; i++)
                        {
                            if (!oldWeather.TimeSlots[i].Equals(newWeatherData.TimeSlots[i]))
                            {
                                hasChanges = true;
                                break;
                            }
                        }
                    }

                    if (hasChanges)
                    {
                        var newVersion = new PageVersion
                        {
                            Content = newJson,
                            Timestamp = DateTime.UtcNow,
                            WatchedPageId = watchedPageId
                        };
                        await _pageVersionRepo.AddAsync(newVersion);

                        var diff = new WeatherChange
                        {
                            OldData = oldWeather,
                            NewData = newWeatherData
                        };

                        var changeLog = new ChangeLog
                        {
                            DiffContent = JsonSerializer.Serialize(diff),
                            SiteType = watchedPage.Type,
                            ChangeDate = DateTime.UtcNow,
                            PageVersion = newVersion,
                            PageVersionId = newVersion.Id
                        };

                        await _changeLogRepo.AddAsync(changeLog);
                        _logger.LogInformation("Changes detected and logged for page {PageId}", watchedPageId);
                    }
                    else
                    {
                        _logger.LogInformation("No changes detected for page {PageId}", watchedPageId);
                    }
                }

                // Update LastChecked timestamp regardless of whether there were changes
                watchedPage.LastChecked = DateTime.UtcNow;
                await _watchedPageRepo.UpdateAsync(watchedPage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking updates for page {PageId}", watchedPageId);
                throw;
            }
        }
    }
}
