using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 13);

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

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "Id", "CategoryId", "ImageUrl", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, 1, "", "Hiking", "" },
                    { 2, 1, "", "Swimming", "" },
                    { 3, 1, "", "Skiing", "" }
                });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6,
                column: "Location",
                value: "Cedars of God, Lebanon");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Modern Lebanese restaurant with upscale ambiance.");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Upscale seafood dining experience.");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Location" },
                values: new object[] { "Modern 5-star city hotel with luxury rooms.", "Verdun, Beirut" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Description", "Location" },
                values: new object[] { "A charming guesthouse in the ancient coastal city of Byblos.", "Byblos, Lebanon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6,
                column: "Location",
                value: "cedars of god Arz, Lebanon");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Babel is a modern Lebanese restaurant known for its upscale ambiance.");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Babel Dbayeh is an upscale Lebanese restaurant known for fresh seafood and elegant dining.");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Location" },
                values: new object[] { "A modern 5-star city hotel in Verdun offering comfortable luxury rooms and business-friendly amenities..", "verdun Beirut, Lebanon" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Description", "Location" },
                values: new object[] { "A charming guesthouse in the ancient coastal city of Byblos", "Byblos , lebanon" });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, 1, "Baatara Gorge Waterfall (Balou’ Balaa) is a spectacular waterfall that drops into a deep sinkhole through three natural rock bridges in Tannourine.\r\n.", "/images/activities/hiking/balou3.jpg", "In Tannourine, North Lebanon", "Balou’ Balaa", 20m },
                    { 2, null, 1, "Discover the breathtaking limestone caves of Jeita.", "/images/activities/hiking/jeita.jpg", "Jeita, Lebanon", "Jeita Grotto Visit", 15m },
                    { 3, null, 1, "Discover the Enchanting Chouwen Lake: A Hidden Gem in Lebanon.", "/images/activities/hiking/chouwen_hike.jpg", "Keserwan District,Mount Lebanon", "Hiking in chouwen", 20m },
                    { 4, null, 1, "Hiking in Wadi Qannoubine offers breathtaking cliffs, ancient monasteries, and peaceful mountain scenery in the heart of Lebanon’s historic valley.\r\n.", "/images/activities/hiking/wadi-qanoubine.jpg", "North Lebanon near Bcharre, Lebanon", "Hiking in wadi qanoubine", 20m },
                    { 11, null, 4, "Al Beiruti is a popular Lebanese restaurant serving traditional mezze and grilled dishes in a lively, authentic atmosphere.", "/images/restaurants/Albeiruti.jpg", "Downtown Beirut, Lebanon", "Al beiruti ", 50m },
                    { 12, null, 4, "Liza is an elegant Lebanese-Mediterranean restaurant in **Beirut** known for beautifully plated traditional dishes with a modern twist.", "/images/restaurants/Liza.jpg", "Achrafieh Beirut, Lebanon", " Liza ", 50m },
                    { 13, null, 4, "Kampaï is a stylish Japanese sushi restaurant in Beirut known for fresh sushi, Asian fusion dishes, and a modern fine-dining atmosphere.", "/images/restaurants/Kampai.jpg", "Palladium Building, Beirut, Lebanon.", " Kampai ", 50m },
                    { 15, null, 3, "A trendy boutique hotel in Badaro known for its artistic design, rooftop pool, and vibrant atmosphere.", "/images/hotels/The Smallville Hotel.jpg", "badaro, Lebanon", "The Smallville Hotel", 250m },
                    { 16, null, 3, "A luxury seaside resort in Beirut featuring pools, private beach access, and resort-style relaxation.", "/images/hotels/Kempinski Summerland Hotel & Resort Beirut.jpg", " Jnah district beirut, Lebanon", "Kempinski hotel", 250m },
                    { 17, null, 3, "A high-end luxury hotel famous for elegant rooms, premium service, and panoramic sea views.", "/images/hotels/Four Seasons Hotel Beirut.jpg", " Downtown beirut, Lebanon", "Four season hotel", 250m },
                    { 18, null, 3, "A refined boutique hotel in Achrafieh offering classic elegance and a quiet upscale experience.", "/images/hotels/Le Gabriel.jpg", " Achrafieh , Lebanon", "Le Gabriel hotel", 250m },
                    { 20, null, 2, "Beit Toureef is a beautifully restored heritage guesthouse", "/images/guesthouses/beitToureef/beitToureef_Overview.jpg", "Gemmayzeh , lebanon", "Beit Toureef", 95m },
                    { 21, null, 2, "Beit Jeddé is a warm village guesthouse located in the historic mountain village of Mtein", "/images/guesthouses/beit-jedde/beitJeddi_Overview.jpg", "Mtein ,Mount lebanon", "Beit jedde", 95m },
                    { 22, null, 2, "Beit El Berbara is a cozy boutique guesthouse located in the seaside town of Barbara, between Byblos and Batroun", "/images/guesthouses/beit-elBarbara/beit-elBarbara.jpg", "Barbara ,lebanon", "Beit elBarbara", 95m }
                });
        }
    }
}
