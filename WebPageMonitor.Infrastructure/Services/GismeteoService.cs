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
using System.Security.Cryptography;
using WebPageMonitor.Infrastructure.Parsers;

namespace WebPageMonitor.Infrastructure.Services
{
    public class GismeteoService
    {
        private readonly IWatchedPageRepository _watchedPageRepo;
        private readonly IPageVersionRepository _pageVersionRepo;
        private readonly IChangeLogRepository _changeLogRepo;
        private readonly GismeteoParser _parser;

        public GismeteoService(
            IWatchedPageRepository watchedPageRepo,
            IPageVersionRepository pageVersionRepo,
            IChangeLogRepository changeLogRepo,
             GismeteoParser parser)  // ← конкретный парсер
        {
            _watchedPageRepo = watchedPageRepo;
            _pageVersionRepo = pageVersionRepo;
            _changeLogRepo = changeLogRepo;
            _parser = parser;
        }

        public async Task CheckUpdatesAsync(int watchedPageId)
        {
            var watchedPage = await _watchedPageRepo.GetByIdAsync(watchedPageId);
            if (watchedPage == null) throw new Exception("Page not found");

            var html = await _parser.GetPageContentAsync(watchedPage.Url);

            var lastVersion = await _pageVersionRepo.GetLatestVersionAsync(watchedPageId);

            var hash = ComputeHash(html);

            if (lastVersion == null || lastVersion.ContentHash != hash)
            {
                var newVersion = new PageVersion
                {
                    Content = html,
                    ContentHash = hash,
                    Timestamp = DateTime.UtcNow,
                    WatchedPageId = watchedPageId
                };
                await _pageVersionRepo.AddAsync(newVersion);

                if (lastVersion != null)
                {
                    var diff = InlineDiffBuilder.Diff(lastVersion.Content, html);
                    var diffContent = SerializeDiff(diff);

                    var changeLog = new ChangeLog
                    {
                        DiffContent = diffContent,
                        SiteType = watchedPage.Type,
                        ChangeDate = DateTime.UtcNow,
                        PageVersion = newVersion
                    };
                    await _changeLogRepo.AddAsync(changeLog);
                }
            }
        }

        private string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        private string SerializeDiff(DiffPaneModel diff)
        {
            return JsonSerializer.Serialize(diff.Lines);
        }
    }
}
