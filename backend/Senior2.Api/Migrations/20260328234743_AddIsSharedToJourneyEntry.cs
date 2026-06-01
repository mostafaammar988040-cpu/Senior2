using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsSharedToJourneyEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "JourneyEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FollowerId = table.Column<int>(type: "int", nullable: false),
                    FollowedId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JourneyEntries_Users_UserId",
                table: "JourneyEntries");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_JourneyEntries_UserId",
                table: "JourneyEntries");

            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "JourneyEntries");
        }
    }
}
