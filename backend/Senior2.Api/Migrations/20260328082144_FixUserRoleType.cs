using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRoleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Travelers",
                table: "SmartItineraryRequest",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Travelers",
                table: "SmartItineraryRequest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
