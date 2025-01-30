using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenshotMonitor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectEmployeeRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundApps",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ForegroundApps",
                table: "Sessions");

            migrationBuilder.CreateTable(
                name: "SessionBackgroundApp",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: true),
                    AppName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionBackgroundApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionBackgroundApp_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SessionForegroundApp",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: true),
                    AppName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionForegroundApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionForegroundApp_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionBackgroundApp_SessionId",
                table: "SessionBackgroundApp",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionForegroundApp_SessionId",
                table: "SessionForegroundApp",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionBackgroundApp");

            migrationBuilder.DropTable(
                name: "SessionForegroundApp");

            migrationBuilder.AddColumn<List<string>>(
                name: "BackgroundApps",
                table: "Sessions",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "ForegroundApps",
                table: "Sessions",
                type: "text[]",
                nullable: true);
        }
    }
}
