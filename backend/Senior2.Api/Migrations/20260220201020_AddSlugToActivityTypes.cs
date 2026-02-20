using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSlugToActivityTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "hiking");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "swimming");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "skiing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "");
        }
    }
}
