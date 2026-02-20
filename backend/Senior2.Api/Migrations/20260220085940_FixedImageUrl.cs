using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixedImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/activities/hiking/chouwen_hike.jpg");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/activities/swimming/swimming.jpg");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/activities/skiing/skiing.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "");
        }
    }
}
