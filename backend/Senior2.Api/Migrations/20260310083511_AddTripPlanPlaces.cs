using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTripPlanPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripPlanPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripPlanId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPlanPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripPlanPlaces_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripPlanPlaces_TripPlans_TripPlanId",
                        column: x => x.TripPlanId,
                        principalTable: "TripPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripPlanPlaces_PlaceId",
                table: "TripPlanPlaces",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlanPlaces_TripPlanId",
                table: "TripPlanPlaces",
                column: "TripPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripPlanPlaces");
        }
    }
}
