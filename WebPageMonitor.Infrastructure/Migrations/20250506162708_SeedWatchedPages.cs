using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebPageMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedWatchedPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WatchedPages",
                columns: new[] { "Id", "CheckInterval", "LastChecked", "Type", "Url" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 0, 0, 0), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://pass.rw.by/ru/route/?from=%D0%9C%D0%B8%D0%BD%D1%81%D0%BA&from_exp=&from_esr=&to=%D0%93%D1%80%D0%BE%D0%B4%D0%BD%D0%BE&to_exp=&to_esr=&front_date=%D1%81%D0%B5%D0%B3%D0%BE%D0%B4%D0%BD%D1%8F&date=today" },
                    { 2, new TimeSpan(0, 0, 0, 0, 0), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "https://www.gismeteo.by/weather-grodno-4243/tomorrow/" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WatchedPages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WatchedPages",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
