using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 41,
                column: "ImageUrl",
                value: "/images/activities/tennis/Movenpick_tennis.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 42,
                column: "ImageUrl",
                value: "/images/activities/tennis/Mt_tennis.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 43,
                column: "ImageUrl",
                value: "/images/activities/tennis/tennisClub.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 45,
                column: "ImageUrl",
                value: "/images/activities/football/triploiStadium.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 41,
                column: "ImageUrl",
                value: "/images/activities/padel/Movenpick_tennis.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 42,
                column: "ImageUrl",
                value: "/images/activities/padel/Mt_tennis.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 43,
                column: "ImageUrl",
                value: "/images/activities/padel/tennisClub.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 45,
                column: "ImageUrl",
                value: "/images/activities/football/tripoliStadium.jpg");
        }
    }
}
