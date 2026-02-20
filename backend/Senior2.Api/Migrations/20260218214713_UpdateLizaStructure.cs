using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLizaStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "/images/restaurants/Liza.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "/images/restaurants/Albeiruti.jpg");
        }
    }
}
