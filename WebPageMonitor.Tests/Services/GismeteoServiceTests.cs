using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Enums;
using WebPageMonitor.Core.Models;
using WebPageMonitor.Core.Parsers;
using WebPageMonitor.Infrastructure.Parsers;
using WebPageMonitor.Infrastructure.Repositories;
using WebPageMonitor.Infrastructure.Services;
using Xunit;

namespace WebPageMonitor.Tests.Services
{
    public class GismeteoServiceTests
    {
        private readonly Mock<IWatchedPageRepository> _mockWatchedPageRepo;
        private readonly Mock<IPageVersionRepository> _mockPageVersionRepo;
        private readonly Mock<IChangeLogRepository> _mockChangeLogRepo;
        private readonly Mock<GismeteoParser> _mockParser;
        private readonly Mock<ILogger<GismeteoService>> _mockLogger;
        private readonly GismeteoService _service;

        public GismeteoServiceTests()
        {
            // Исправление: Добавляем мок ILogger для GismeteoParser
            var mockParserLogger = new Mock<ILogger<GismeteoParser>>();

            _mockWatchedPageRepo = new Mock<IWatchedPageRepository>();
            _mockPageVersionRepo = new Mock<IPageVersionRepository>();
            _mockChangeLogRepo = new Mock<IChangeLogRepository>();

            // Исправление: Передаем зависимости в конструктор GismeteoParser
            _mockParser = new Mock<GismeteoParser>(mockParserLogger.Object);

            _mockLogger = new Mock<ILogger<GismeteoService>>();

            _service = new GismeteoService(
                _mockWatchedPageRepo.Object,
                _mockPageVersionRepo.Object,
                _mockChangeLogRepo.Object,
                _mockParser.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task CheckUpdatesAsync_WhenPageNotFound_ThrowsException()
        {
            // Arrange
            int pageId = 1;
            _mockWatchedPageRepo.Setup(repo => repo.GetByIdAsync(pageId))
                .ReturnsAsync((WatchedPage?)null); // Явное указание nullable

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CheckUpdatesAsync(pageId));
        }

        [Fact]
        public async Task CheckUpdatesAsync_WhenFirstVersion_CreatesVersionWithoutChangeLog()
        {
            // Arrange
            var watchedPage = new WatchedPage
            {
                Id = 1,
                Url = "http://test.com",
                Type = WebSiteType.Gismeteo,
                LastChecked = null // Теперь nullable
            };

            var weatherData = new WeatherData
            {
                TimeSlots = new List<TimeSlotData>
                {
                    new TimeSlotData { Time = "12:00", Temperature = "20" }
                }
            };

            _mockWatchedPageRepo.Setup(repo => repo.GetByIdAsync(watchedPage.Id))
                .ReturnsAsync(watchedPage);

            _mockPageVersionRepo.Setup(repo => repo.GetLatestVersionAsync(watchedPage.Id))
                .ReturnsAsync((PageVersion?)null); // Явное указание nullable

            _mockParser.Setup(p => p.GetWeatherDataAsync(watchedPage.Url))
                .ReturnsAsync(weatherData);

            // Act
            await _service.CheckUpdatesAsync(watchedPage.Id);

            // Assert
            _mockPageVersionRepo.Verify(repo => repo.AddAsync(It.IsAny<PageVersion>()), Times.Once);
            _mockChangeLogRepo.Verify(repo => repo.AddAsync(It.IsAny<ChangeLog>()), Times.Never);
            _mockWatchedPageRepo.Verify(repo => repo.UpdateAsync(It.Is<WatchedPage>(wp =>
                wp.Id == watchedPage.Id && wp.LastChecked.HasValue)), Times.Once); // Используем HasValue
        }

        [Fact]
        public async Task CheckUpdatesAsync_WhenNoChanges_DoesNotCreateVersionOrChangeLog()
        {
            // Arrange
            var watchedPage = new WatchedPage
            {
                Id = 1,
                Url = "http://test.com",
                Type = WebSiteType.Gismeteo,
                LastChecked = null
            };

            var weatherData = new WeatherData
            {
                TimeSlots = new List<TimeSlotData>
                {
                    new TimeSlotData { Time = "12:00", Temperature = "20" }
                }
            };

            var existingVersion = new PageVersion
            {
                Id = 1,
                Content = JsonSerializer.Serialize(weatherData),
                Timestamp = DateTime.UtcNow.AddHours(-1),
                WatchedPageId = watchedPage.Id
            };

            _mockWatchedPageRepo.Setup(repo => repo.GetByIdAsync(watchedPage.Id))
                .ReturnsAsync(watchedPage);

            _mockPageVersionRepo.Setup(repo => repo.GetLatestVersionAsync(watchedPage.Id))
                .ReturnsAsync(existingVersion);

            _mockParser.Setup(p => p.GetWeatherDataAsync(watchedPage.Url))
                .ReturnsAsync(weatherData);

            // Act
            await _service.CheckUpdatesAsync(watchedPage.Id);

            // Assert
            _mockPageVersionRepo.Verify(repo => repo.AddAsync(It.IsAny<PageVersion>()), Times.Never);
            _mockChangeLogRepo.Verify(repo => repo.AddAsync(It.IsAny<ChangeLog>()), Times.Never);
            _mockWatchedPageRepo.Verify(repo => repo.UpdateAsync(It.Is<WatchedPage>(wp =>
                wp.Id == watchedPage.Id && wp.LastChecked.HasValue)), Times.Once);
        }

        [Fact]
        public async Task CheckUpdatesAsync_WhenChangesDetected_CreatesVersionAndChangeLog()
        {
            // Arrange
            var watchedPage = new WatchedPage
            {
                Id = 1,
                Url = "http://test.com",
                Type = WebSiteType.Gismeteo,
                LastChecked = null
            };

            var oldWeatherData = new WeatherData
            {
                TimeSlots = new List<TimeSlotData>
                {
                    new TimeSlotData { Time = "12:00", Temperature = "20" }
                }
            };

            var newWeatherData = new WeatherData
            {
                TimeSlots = new List<TimeSlotData>
                {
                    new TimeSlotData { Time = "12:00", Temperature = "25" } // Изменение температуры
                }
            };

            var existingVersion = new PageVersion
            {
                Id = 1,
                Content = JsonSerializer.Serialize(oldWeatherData),
                Timestamp = DateTime.UtcNow.AddHours(-1),
                WatchedPageId = watchedPage.Id
            };

            _mockWatchedPageRepo.Setup(repo => repo.GetByIdAsync(watchedPage.Id))
                .ReturnsAsync(watchedPage);

            _mockPageVersionRepo.Setup(repo => repo.GetLatestVersionAsync(watchedPage.Id))
                .ReturnsAsync(existingVersion);

            _mockParser.Setup(p => p.GetWeatherDataAsync(watchedPage.Url))
                .ReturnsAsync(newWeatherData);

            // Act
            await _service.CheckUpdatesAsync(watchedPage.Id);

            // Assert
            _mockPageVersionRepo.Verify(repo => repo.AddAsync(It.Is<PageVersion>(pv =>
                pv.WatchedPageId == watchedPage.Id)), Times.Once);

            _mockChangeLogRepo.Verify(repo => repo.AddAsync(It.Is<ChangeLog>(cl =>
                cl.SiteType == WebSiteType.Gismeteo &&
                cl.PageVersion != null)), Times.Once);

            _mockWatchedPageRepo.Verify(repo => repo.UpdateAsync(It.Is<WatchedPage>(wp =>
                wp.Id == watchedPage.Id && wp.LastChecked.HasValue)), Times.Once);
        }
    }
}