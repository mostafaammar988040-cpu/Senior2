using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlacesStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Location", "Name" },
                values: new object[] { "Baatara Gorge Waterfall (Balou’ Balaa) is a spectacular waterfall that drops into a deep sinkhole through three natural rock bridges in Tannourine.\r\n.", "In Tannourine, North Lebanon", "Balou’ Balaa" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 1, "Discover the Enchanting Chouwen Lake: A Hidden Gem in Lebanon.", "/images/activities/hiking/chouwen_hike.jpg", "Keserwan District,Mount Lebanon", "Hiking in chouwen", 20m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 1, "Hiking in Wadi Qannoubine offers breathtaking cliffs, ancient monasteries, and peaceful mountain scenery in the heart of Lebanon’s historic valley.\r\n.", "/images/activities/hiking/wadi-qanoubine.jpg", "North Lebanon near Bcharre, Lebanon", "Hiking in wadi qanoubine", 20m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 2, "Traditional Lebanese mountain guesthouse with scenic views.", "/images/guesthouses/beit-trad/beitTrad_livingRoom.jpg", "Broummana, Lebanon", "Beit Trad Guesthouse", 80m });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 6, null, 2, "Charming stay surrounded by nature.", "/images/guesthouses/charme.jpg", "Beit Mery, Lebanon", "Charme Guesthouse", 95m },
                    { 7, null, 3, "Luxury 5-star hotel overlooking the Mediterranean Sea.", "/images/hotels/phoenicia.jpg", "Beirut, Lebanon", "Phoenicia Hotel", 250m },
                    { 8, null, 4, "Authentic Lebanese fine dining experience.", "/images/restaurants/emsherif.jpg", "Downtown Beirut, Lebanon", "Em Sherif Restaurant", 50m },
                    { 9, null, 4, "Babel is a modern Lebanese restaurant known for its upscale ambiance.", "/images/restaurants/Bebabel.jpg", "Downtown Beirut, Lebanon", "Bebabel", 50m },
                    { 10, null, 4, "Babel Dbayeh is an upscale Lebanese restaurant known for fresh seafood and elegant dining.", "/images/restaurants/Babel.jpg", "Dbayeh, Lebanon", "Babel Bay", 50m },
                    { 11, null, 4, "Al Beiruti is a popular Lebanese restaurant serving traditional mezze and grilled dishes in a lively, authentic atmosphere.", "/images/restaurants/Albeiruti.jpg", "Downtown Beirut, Lebanon", "Al beiruti ", 50m },
                    { 12, null, 4, "Liza is an elegant Lebanese-Mediterranean restaurant in **Beirut** known for beautifully plated traditional dishes with a modern twist.", "/images/restaurants/Albeiruti.jpg", "Achrafieh Beirut, Lebanon", " Liza ", 50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Location", "Name" },
                values: new object[] { "Explore beautiful cedar forests in the Chouf mountains.", "Chouf, Lebanon", "Hiking in Chouf" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 2, "Traditional Lebanese mountain guesthouse with scenic views.", "/images/guesthouses/beit-trad/beitTrad_livingRoom.jpg", "Broummana, Lebanon", "Beit Trad Guesthouse", 80m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 3, "Luxury 5-star hotel overlooking the Mediterranean Sea.", "/images/hotels/phoenicia.jpg", "Beirut, Lebanon", "Phoenicia Hotel", 250m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 4, "Authentic Lebanese fine dining experience.", "/images/restaurants/emsherif.jpg", "Beirut, Lebanon", "Em Sherif Restaurant", 50m });
        }
    }
}
