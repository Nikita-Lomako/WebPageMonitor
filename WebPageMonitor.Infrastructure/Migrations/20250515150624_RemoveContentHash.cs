using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPageMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveContentHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentHash",
                table: "PageVersions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentHash",
                table: "PageVersions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
