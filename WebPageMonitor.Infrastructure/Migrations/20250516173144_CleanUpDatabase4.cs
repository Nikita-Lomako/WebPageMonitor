using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebPageMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanUpDatabase4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Отключить проверку внешних ключей
            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

            // Очистить все таблицы
            migrationBuilder.Sql("DELETE FROM ChangeLogs");
            migrationBuilder.Sql("DELETE FROM PageVersions");
            migrationBuilder.Sql("DELETE FROM WatchedPages");

            // Сбросить автоинкремент для всех таблиц
            migrationBuilder.Sql("DBCC CHECKIDENT ('ChangeLogs', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('PageVersions', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('WatchedPages', RESEED, 0)");

            // Включить проверку внешних ключей
            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");

            // Добавить начальные данные
            migrationBuilder.InsertData(
                table: "WatchedPages",
                columns: new[] { "Id", "CheckInterval", "LastChecked", "Type", "Url" },
                values: new object[,]
                {
                    {
                        1,
                        new TimeSpan(0, 0, 0, 0, 0),
                        new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                        0,
                        "https://www.gismeteo.by/weather-grodno-4243/tomorrow/"
                    }
                });

            migrationBuilder.InsertData(
                table: "PageVersions",
                columns: new[] { "Id", "Content", "Timestamp", "WatchedPageId" },
                values: new object[,]
                {
                    { 1, "{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"5°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"90%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"94%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"}", new DateTime(2025, 5, 16, 18, 20, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, "{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"6°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"89%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"Ю\",\"Humidity\":\"92%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"}", new DateTime(2025, 5, 16, 19, 20, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.InsertData(
                table: "ChangeLogs",
                columns: new[] { "Id", "ChangeDate", "DiffContent", "PageVersionId", "SiteType" },
                values: new object[] { 1, new DateTime(2025, 5, 16, 19, 20, 0, 0, DateTimeKind.Utc), "{\"OldData\":{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"5°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"90%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"94%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"},\"NewData\":{\"TimeSlots\":[{\"Time\":\"0:00\",\"Temperature\":\"6°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"89%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"3:00\",\"Temperature\":\"4°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"Ю\",\"Humidity\":\"92%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"6:00\",\"Temperature\":\"3°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"99%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"9:00\",\"Temperature\":\"7°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"86%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"12:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"74%\",\"Pressure\":\"744 мм рт.ст.\",\"Precipitation\":\"0 мм\"},{\"Time\":\"15:00\",\"Temperature\":\"10°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"74%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"0,9 мм\"},{\"Time\":\"18:00\",\"Temperature\":\"11°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"ЮВ\",\"Humidity\":\"77%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,6 мм\"},{\"Time\":\"21:00\",\"Temperature\":\"9°C\",\"WindSpeed\":\" м/с\",\"WindDirection\":\"В\",\"Humidity\":\"85%\",\"Pressure\":\"743 мм рт.ст.\",\"Precipitation\":\"1,8 мм\"}],\"ObservationTime\":\"2025-05-16T16:36:45.3837758Z\"}}", 2, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChangeLogs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PageVersions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PageVersions",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
