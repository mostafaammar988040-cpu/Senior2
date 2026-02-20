using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlaceImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/activities/hiking/balou3.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/guesthouses/beit-trad/beitTrad_livingRoom.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/activities/hiking/hiking.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/guesthouses/beittrad.jpg");
        }
    }
}
