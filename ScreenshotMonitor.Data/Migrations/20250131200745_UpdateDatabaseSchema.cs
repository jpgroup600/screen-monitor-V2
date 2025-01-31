using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenshotMonitor.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployees_Projects_ProjectId",
                table: "ProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployees_Users_EmployeeId",
                table: "ProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_AdminId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Sessions_SessionId",
                table: "Screenshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionBackgroundApp_Sessions_SessionId",
                table: "SessionBackgroundApp");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionForegroundApp_Sessions_SessionId",
                table: "SessionForegroundApp");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Projects_ProjectId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Users_EmployeeId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionForegroundApp",
                table: "SessionForegroundApp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionBackgroundApp",
                table: "SessionBackgroundApp");

            migrationBuilder.RenameTable(
                name: "SessionForegroundApp",
                newName: "SessionForegroundApps");

            migrationBuilder.RenameTable(
                name: "SessionBackgroundApp",
                newName: "SessionBackgroundApps");

            migrationBuilder.RenameIndex(
                name: "IX_SessionForegroundApp_SessionId",
                table: "SessionForegroundApps",
                newName: "IX_SessionForegroundApps_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionBackgroundApp_SessionId",
                table: "SessionBackgroundApps",
                newName: "IX_SessionBackgroundApps_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionForegroundApps",
                table: "SessionForegroundApps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionBackgroundApps",
                table: "SessionBackgroundApps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployees_Projects_ProjectId",
                table: "ProjectEmployees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployees_Users_EmployeeId",
                table: "ProjectEmployees",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_AdminId",
                table: "Projects",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Sessions_SessionId",
                table: "Screenshots",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionBackgroundApps_Sessions_SessionId",
                table: "SessionBackgroundApps",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionForegroundApps_Sessions_SessionId",
                table: "SessionForegroundApps",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Projects_ProjectId",
                table: "Sessions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Users_EmployeeId",
                table: "Sessions",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployees_Projects_ProjectId",
                table: "ProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployees_Users_EmployeeId",
                table: "ProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_AdminId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Sessions_SessionId",
                table: "Screenshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionBackgroundApps_Sessions_SessionId",
                table: "SessionBackgroundApps");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionForegroundApps_Sessions_SessionId",
                table: "SessionForegroundApps");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Projects_ProjectId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Users_EmployeeId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionForegroundApps",
                table: "SessionForegroundApps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionBackgroundApps",
                table: "SessionBackgroundApps");

            migrationBuilder.RenameTable(
                name: "SessionForegroundApps",
                newName: "SessionForegroundApp");

            migrationBuilder.RenameTable(
                name: "SessionBackgroundApps",
                newName: "SessionBackgroundApp");

            migrationBuilder.RenameIndex(
                name: "IX_SessionForegroundApps_SessionId",
                table: "SessionForegroundApp",
                newName: "IX_SessionForegroundApp_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionBackgroundApps_SessionId",
                table: "SessionBackgroundApp",
                newName: "IX_SessionBackgroundApp_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionForegroundApp",
                table: "SessionForegroundApp",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionBackgroundApp",
                table: "SessionBackgroundApp",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployees_Projects_ProjectId",
                table: "ProjectEmployees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployees_Users_EmployeeId",
                table: "ProjectEmployees",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_AdminId",
                table: "Projects",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Sessions_SessionId",
                table: "Screenshots",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionBackgroundApp_Sessions_SessionId",
                table: "SessionBackgroundApp",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionForegroundApp_Sessions_SessionId",
                table: "SessionForegroundApp",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Projects_ProjectId",
                table: "Sessions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Users_EmployeeId",
                table: "Sessions",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
