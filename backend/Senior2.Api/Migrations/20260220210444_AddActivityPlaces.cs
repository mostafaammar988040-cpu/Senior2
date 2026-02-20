using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 19, 2, 1, "Popular swimming destination in Beirut.", "/images/activities/swimming/sporting.jpg", "Beirut", "Sporting Beach", 25m },
                    { 20, 3, 1, "Best skiing destination in Lebanon.", "/images/activities/skiing/mzaar.jpg", "Kfardebian", "Mzaar Ski Resort", 60m },
                    { 21, 1, 1, "Beautiful hiking area with river pools.", "/images/activities/hiking/chouwen.jpg", "Jbeil", "Chouwen Hiking Trail", 0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 21);
        }
    }
}
