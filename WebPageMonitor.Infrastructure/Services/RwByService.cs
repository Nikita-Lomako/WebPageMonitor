using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Parsers;
using WebPageMonitor.Infrastructure.Parsers;
using WebPageMonitor.Infrastructure.Repositories;

namespace WebPageMonitor.Infrastructure.Services
{
    public class RwByService
    {
        private readonly IWatchedPageRepository _watchedPageRepo;
        private readonly IPageVersionRepository _pageVersionRepo;
        private readonly IChangeLogRepository _changeLogRepo;
        private readonly RwByParser _parser;

        public RwByService(
            IWatchedPageRepository watchedPageRepo,
            IPageVersionRepository pageVersionRepo,
            IChangeLogRepository changeLogRepo,
            RwByParser parser)
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
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        private string SerializeDiff(DiffPaneModel diff)
        {
            return System.Text.Json.JsonSerializer.Serialize(diff.Lines);
        }
    }
}
