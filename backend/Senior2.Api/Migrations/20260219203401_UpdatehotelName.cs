using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatehotelName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "Kempinski hotel");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "Four season hotel");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "Le Gabriel hotel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "The Smallville Hotel");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "The Smallville Hotel");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "The Smallville Hotel");
        }
    }
}
