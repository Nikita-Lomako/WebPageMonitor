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
                   Url = "https://pass.rw.by/ru/route/?from=%D0%9C%D0%B8%D0%BD%D1%81%D0%BA&from_exp=&from_esr=&to=%D0%93%D1%80%D0%BE%D0%B4%D0%BD%D0%BE&to_exp=&to_esr=&front_date=%D1%81%D0%B5%D0%B3%D0%BE%D0%B4%D0%BD%D1%8F&date=today",
                   Type = WebSiteType.Train
               },
               new WatchedPage
               {
                   Id = 2,
                   Url = "https://www.gismeteo.by/weather-grodno-4243/tomorrow/",
                   Type = WebSiteType.Weather
               }
           );
        }
    }
}

