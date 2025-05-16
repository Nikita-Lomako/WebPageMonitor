using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebPageMonitor.Core.Entities;
using WebPageMonitor.Core.Enums;

namespace WebPageMonitor.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<WatchedPage> WatchedPages { get; set; }
        public DbSet<PageVersion> PageVersions { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация отношений

            modelBuilder.Entity<PageVersion>()
                .HasOne(pv => pv.WatchedPage)
                .WithMany(wp => wp.Versions)
                .HasForeignKey(pv => pv.WatchedPageId);

            modelBuilder.Entity<ChangeLog>()
                .HasOne(cl => cl.PageVersion)
                .WithMany()
                .HasForeignKey(cl => cl.PageVersionId);

            modelBuilder.Entity<WatchedPage>().HasData(
               new WatchedPage
               {
                   Id = 1,
                   Url = "https://www.gismeteo.by/weather-grodno-4243/tomorrow/",
                   Type = WebSiteType.Gismeteo
               }
           );

            modelBuilder.Entity<PageVersion>().HasData(
                new PageVersion
                {
                    Id = 1,
                    Content = "{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"5°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"90%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"94%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"}",
                    Timestamp = new DateTime(2025, 5, 16, 18, 20, 0, DateTimeKind.Utc),
                    WatchedPageId = 1
                },
                new PageVersion
                {
                    Id = 2,
                    Content = "{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"6°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"89%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"Ю\",\"Humidity\":\"92%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"}",
                    Timestamp = new DateTime(2025, 5, 16, 19, 20, 0, DateTimeKind.Utc),
                    WatchedPageId = 1
                }
            );

            modelBuilder.Entity<ChangeLog>().HasData(
                new ChangeLog
                {
                    Id = 1,
                    DiffContent = "{\"OldData\":{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"5°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"90%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"94%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"},\"NewData\":{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"6°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"89%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"Ю\",\"Humidity\":\"92%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"}}",
                    SiteType = WebSiteType.Gismeteo,
                    ChangeDate = new DateTime(2025, 5, 16, 19, 20, 0, DateTimeKind.Utc),
                    PageVersionId = 2
                }
            );
        }
    }
}

