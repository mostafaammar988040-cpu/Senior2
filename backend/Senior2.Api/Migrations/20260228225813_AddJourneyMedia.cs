using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddJourneyMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JourneyEntries_Users_UserId",
                table: "JourneyEntries");

            migrationBuilder.DropIndex(
                name: "IX_JourneyEntries_UserId",
                table: "JourneyEntries");

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "JourneyEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaUrl",
                table: "JourneyEntries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "JourneyEntries");

            migrationBuilder.DropColumn(
                name: "MediaUrl",
                table: "JourneyEntries");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyEntries_UserId",
                table: "JourneyEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JourneyEntries_Users_UserId",
                table: "JourneyEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
