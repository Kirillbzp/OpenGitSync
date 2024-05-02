using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class SyncSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_SyncSettings_SyngSettingsId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_SyngSettingsId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "SyncSettingId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "SyngSettingsId",
                table: "Schedule");
        }
    }
}
