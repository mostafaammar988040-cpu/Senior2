using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGuesthouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageUrl", "Location" },
                values: new object[] { "/images/guesthouses/charme guesthouse/charme_overview.jpg", "cedars of god Arz, Lebanon" });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 19, null, 2, "A charming guesthouse in the ancient coastal city of Byblos", "/images/guesthouses/Beit Faris.jpg", "Byblos , lebanon", "Beit Faris", 95m },
                    { 20, null, 2, "Beit Toureef is a beautifully restored heritage guesthouse", "/images/guesthouses/beitToureef/beitToureef_Overview.jpg", "Gemmayzeh , lebanon", "Beit Toureef", 95m },
                    { 21, null, 2, "Beit Jeddé is a warm village guesthouse located in the historic mountain village of Mtein", "/images/guesthouses/beit-jedde/beitJeddi_Overview.jpg", "Mtein ,Mount lebanon", "Beit jedde", 95m },
                    { 22, null, 2, "Beit El Berbara is a cozy boutique guesthouse located in the seaside town of Barbara, between Byblos and Batroun", "/images/guesthouses/beit-elBarbara/beit-elBarbara.jpg", "Barbara ,lebanon", "Beit elBarbara", 95m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 19);

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

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageUrl", "Location" },
                values: new object[] { "/images/guesthouses/charme.jpg", "Beit Mery, Lebanon" });
        }
    }
}
