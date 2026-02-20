using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class ActivityCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "CategoryId", "ImageUrl", "Name", "Slug" },
                values: new object[,]
                {
                    { 4, 1, "/images/activities/football/football.jpg", "Football", "Football" },
                    { 5, 1, "/images/activities/padel/padel.jpg", "Padel", "Padel" },
                    { 6, 1, "/images/activities/tennis/tennis.jpg", "Tennis", "Teniis" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
