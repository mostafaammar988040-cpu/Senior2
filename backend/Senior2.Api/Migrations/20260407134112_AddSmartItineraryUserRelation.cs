using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartItineraryUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SmartItineraryRequest_UserId",
                table: "SmartItineraryRequest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartItineraryRequest_Users_UserId",
                table: "SmartItineraryRequest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartItineraryRequest_Users_UserId",
                table: "SmartItineraryRequest");

            migrationBuilder.DropIndex(
                name: "IX_SmartItineraryRequest_UserId",
                table: "SmartItineraryRequest");
        }
    }
}
