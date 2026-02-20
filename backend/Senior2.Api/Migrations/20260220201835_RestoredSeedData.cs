using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class RestoredSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "hiking");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "swimming");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "skiing");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { "Warm village guesthouse in Mtein.", "/images/guesthouses/beit-jedde/beitJeddi_Overview.jpg", "Mtein, Lebanon", "Beit Jeddé", 95m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "ImageUrl", "Location", "Name" },
                values: new object[] { "Cozy boutique guesthouse by the sea.", "/images/guesthouses/beit-elBarbara/beit-elBarbara.jpg", "Barbara, Lebanon", "Beit El Berbara" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Location" },
                values: new object[] { "Luxury 5-star hotel.", "Beirut" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 3, "Modern business hotel.", "/images/hotels/radisson.jpg", "Verdun Beirut", "Radisson Blu Hotel", 250m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 3, "Trendy boutique hotel.", "/images/hotels/The Smallville Hotel.jpg", "Badaro", "The Smallville Hotel", 250m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 3, "Luxury seaside resort.", "/images/hotels/Kempinski Summerland Hotel & Resort Beirut.jpg", "Jnah Beirut", "Kempinski Summerland", 250m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 4, "Modern Lebanese restaurant.", "/images/restaurants/Bebabel.jpg", "Beirut", "Bebabel", 50m });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, 2, "Traditional Lebanese mountain guesthouse.", "/images/guesthouses/beit-trad/beitTrad_livingRoom.jpg", "Broummana, Lebanon", "Beit Trad Guesthouse", 80m },
                    { 2, null, 2, "Charming stay surrounded by nature.", "/images/guesthouses/charme guesthouse/charme_overview.jpg", "Cedars, Lebanon", "Charme Guesthouse", 95m },
                    { 3, null, 2, "A charming guesthouse in Byblos.", "/images/guesthouses/Beit Faris.jpg", "Byblos, Lebanon", "Beit Faris", 95m },
                    { 4, null, 2, "Beautiful heritage guesthouse.", "/images/guesthouses/beitToureef/beitToureef_Overview.jpg", "Gemmayzeh, Lebanon", "Beit Toureef", 95m },
                    { 11, null, 3, "High-end luxury hotel.", "/images/hotels/Four Seasons Hotel Beirut.jpg", "Downtown Beirut", "Four Seasons Hotel", 250m },
                    { 12, null, 3, "Elegant boutique hotel.", "/images/hotels/Le Gabriel.jpg", "Achrafieh", "Le Gabriel Hotel", 250m },
                    { 13, null, 4, "Authentic Lebanese fine dining.", "/images/restaurants/emsherif.jpg", "Beirut", "Em Sherif", 50m },
                    { 15, null, 4, "Upscale seafood dining.", "/images/restaurants/Babel.jpg", "Dbayeh", "Babel Bay", 50m },
                    { 16, null, 4, "Traditional mezze and grills.", "/images/restaurants/Albeiruti.jpg", "Beirut", "Al Beiruti", 50m },
                    { 17, null, 4, "Elegant Lebanese-Mediterranean.", "/images/restaurants/Liza.jpg", "Achrafieh", "Liza", 50m },
                    { 18, null, 4, "Japanese sushi & fusion.", "/images/restaurants/Kampai.jpg", "Beirut", "Kampai", 50m }
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

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { "Traditional Lebanese mountain guesthouse with scenic views.", "/images/guesthouses/beit-trad/beitTrad_livingRoom.jpg", "Broummana, Lebanon", "Beit Trad Guesthouse", 80m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "ImageUrl", "Location", "Name" },
                values: new object[] { "Charming stay surrounded by nature.", "/images/guesthouses/charme guesthouse/charme_overview.jpg", "Cedars of God, Lebanon", "Charme Guesthouse" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Location" },
                values: new object[] { "Luxury 5-star hotel overlooking the Mediterranean Sea.", "Beirut, Lebanon" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 4, "Authentic Lebanese fine dining experience.", "/images/restaurants/emsherif.jpg", "Downtown Beirut, Lebanon", "Em Sherif Restaurant", 50m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 4, "Modern Lebanese restaurant with upscale ambiance.", "/images/restaurants/Bebabel.jpg", "Downtown Beirut, Lebanon", "Bebabel", 50m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 4, "Upscale seafood dining experience.", "/images/restaurants/Babel.jpg", "Dbayeh, Lebanon", "Babel Bay", 50m });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 3, "Modern 5-star city hotel with luxury rooms.", "/images/hotels/radisson.jpg", "Verdun, Beirut", "Radisson Blu Hotel", 250m });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[] { 19, null, 2, "A charming guesthouse in the ancient coastal city of Byblos.", "/images/guesthouses/Beit Faris.jpg", "Byblos, Lebanon", "Beit Faris", 95m });
        }
    }
}
