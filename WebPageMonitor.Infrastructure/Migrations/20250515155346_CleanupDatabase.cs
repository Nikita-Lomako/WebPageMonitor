using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPageMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanupDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        BEGIN TRANSACTION;

        -- Отключить проверку внешних ключей
        EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

        -- Очистить таблицы
        DELETE FROM ChangeLogs;
        DELETE FROM PageVersions;
        DELETE FROM WatchedPages WHERE Id NOT IN (1, 2);

        -- Включить проверку внешних ключей
        EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';

        COMMIT TRANSACTION;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
