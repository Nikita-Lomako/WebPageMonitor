using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPageMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanUpDatabase3 : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
