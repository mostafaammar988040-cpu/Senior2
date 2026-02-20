using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 13, null, 4, "Kampaï is a stylish Japanese sushi restaurant in Beirut known for fresh sushi, Asian fusion dishes, and a modern fine-dining atmosphere.", "/images/restaurants/Kampai.jpg", "Palladium Building, Beirut, Lebanon.", " Kampai ", 50m },
                    { 14, null, 3, "A modern 5-star city hotel in Verdun offering comfortable luxury rooms and business-friendly amenities..", "/images/hotels/radisson.jpg", "verdun Beirut, Lebanon", "Radisson Blu Hotel", 250m },
                    { 15, null, 3, "A trendy boutique hotel in Badaro known for its artistic design, rooftop pool, and vibrant atmosphere.", "/images/hotels/The Smallville Hotel.jpg", "badaro, Lebanon", "The Smallville Hotel", 250m },
                    { 16, null, 3, "A luxury seaside resort in Beirut featuring pools, private beach access, and resort-style relaxation.", "/images/hotels/Kempinski Summerland Hotel & Resort Beirut.jpg", " Jnah district beirut, Lebanon", "The Smallville Hotel", 250m },
                    { 17, null, 3, "A high-end luxury hotel famous for elegant rooms, premium service, and panoramic sea views.", "/images/hotels/Four Seasons Hotel Beirut.jpg", " Downtown beirut, Lebanon", "The Smallville Hotel", 250m },
                    { 18, null, 3, "A refined boutique hotel in Achrafieh offering classic elegance and a quiet upscale experience.", "/images/hotels/Le Gabriel.jpg", " Achrafieh , Lebanon", "The Smallville Hotel", 250m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
