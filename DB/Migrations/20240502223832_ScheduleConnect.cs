using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleConnect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_SyncSettings_SyngSettingsId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_SyncSettings_Schedule_ScheduleId",
                table: "SyncSettings");

            migrationBuilder.DropIndex(
                name: "IX_SyncSettings_ScheduleId",
                table: "SyncSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_SyngSettingsId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "SyncSettingId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "SyngSettingsId",
                table: "Schedule");

            migrationBuilder.RenameTable(
                name: "Schedule",
                newName: "Schedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SyncSettings_ScheduleId",
                table: "SyncSettings",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SyncSettings_Schedules_ScheduleId",
                table: "SyncSettings",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyncSettings_Schedules_ScheduleId",
                table: "SyncSettings");

            migrationBuilder.DropIndex(
                name: "IX_SyncSettings_ScheduleId",
                table: "SyncSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedule");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Schedule",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SyncSettingId",
                table: "Schedule",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SyngSettingsId",
                table: "Schedule",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SyncSettings_ScheduleId",
                table: "SyncSettings",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SyngSettingsId",
                table: "Schedule",
                column: "SyngSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_SyncSettings_SyngSettingsId",
                table: "Schedule",
                column: "SyngSettingsId",
                principalTable: "SyncSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SyncSettings_Schedule_ScheduleId",
                table: "SyncSettings",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
