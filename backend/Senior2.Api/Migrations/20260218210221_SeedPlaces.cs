using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, 1, "Explore beautiful cedar forests in the Chouf mountains.", "/images/activities/hiking/hiking.jpg", "Chouf, Lebanon", "Hiking in Chouf", 20m },
                    { 2, null, 1, "Discover the breathtaking limestone caves of Jeita.", "/images/activities/hiking/jeita.jpg", "Jeita, Lebanon", "Jeita Grotto Visit", 15m },
                    { 3, null, 2, "Traditional Lebanese mountain guesthouse with scenic views.", "/images/guesthouses/beittrad.jpg", "Broummana, Lebanon", "Beit Trad Guesthouse", 80m },
                    { 4, null, 3, "Luxury 5-star hotel overlooking the Mediterranean Sea.", "/images/hotels/phoenicia.jpg", "Beirut, Lebanon", "Phoenicia Hotel", 250m },
                    { 5, null, 4, "Authentic Lebanese fine dining experience.", "/images/restaurants/emsherif.jpg", "Beirut, Lebanon", "Em Sherif Restaurant", 50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
