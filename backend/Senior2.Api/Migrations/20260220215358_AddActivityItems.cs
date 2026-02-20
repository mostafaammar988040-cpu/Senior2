using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Senior2.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Slug",
                value: "football");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Slug",
                value: "padel");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Slug",
                value: "tennis");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/images/activities/swimming/movenpick.jpg", "Movenpick Beach" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 20,
                column: "ImageUrl",
                value: "/images/activities/skiing/mzaarSkiResort.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 21,
                column: "ImageUrl",
                value: "/images/activities/hiking/chouwen_hike.jpg");

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "ActivityTypeId", "CategoryId", "Description", "ImageUrl", "Location", "Name", "Price" },
                values: new object[,]
                {
                    { 22, 2, 1, "Popular swimming destination in Beirut.", "/images/activities/swimming/sporting.jpg", "Beirut", "Sporting Beach", 25m },
                    { 23, 2, 1, "Popular swimming destination in batroun.", "/images/activities/swimming/blubay.jpg", "batroun", "Blubay Beach", 25m },
                    { 24, 2, 1, "Popular swimming destination in tyree.", "/images/activities/swimming/tyree.jpg", "South Lebanon (Sour)", "Tyree Beach", 25m },
                    { 25, 2, 1, "Popular swimming destination in anfeh.", "/images/activities/swimming/tahetelrich.jpg", "Anfeh (North Lebanon)", "Tahet el rich Beach", 25m },
                    { 26, 2, 1, "Popular swimming destination in jiyeh.", "/images/activities/swimming/lazyb.jpg", "Jiyeh, South of Beirut (Mount Lebanon)", "Lazy B  Beach", 25m },
                    { 27, 3, 1, "Best skiing destination in Lebanon.", "/images/activities/skiing/cedarsSkiResort.jpg", "arz", "Cedars Ski Resort", 60m },
                    { 28, 3, 1, "Best skiing destination in Lebanon.", "/images/activities/skiing/laqlouqSkiResort.jpg", "laqlouq", "Laqlouq Ski Resort", 60m },
                    { 29, 3, 1, "Best skiing destination in Lebanon.", "/images/activities/skiing/zaarour.jpg", "zaarour", "Zaarour Ski Resort", 60m },
                    { 30, 1, 1, "A UNESCO-listed valley known for dramatic cliffs, ancient monasteries, and peaceful nature trails — one of the most iconic hikes in Lebanon.", "/images/activities/hiking/wadi-qadisha.jpg", "North Lebanon", "Wadi Qadisha Hiking Trail", 0m },
                    { 31, 1, 1, "A beautiful cedar forest with marked trails, fresh air, and panoramic mountain views — perfect for nature lovers.", "/images/activities/hiking/tannourine.jpg", "North Lebanon", "Tannourine hiking Trail", 0m },
                    { 32, 1, 1, "A spectacular natural sinkhole with waterfalls and bridges — short hike but incredible views, especially in spring.", "/images/activities/hiking/balou3.jpg", "North Lebanon", "Balou balaa hiking Trail", 0m },
                    { 33, 1, 1, "A large protected area with rich biodiversity, cool weather, and multiple hiking trails ranging from easy to advanced.", "/images/activities/hiking/ehden.jpg", "North Lebanon", "Ehden  hiking Trail", 0m },
                    { 34, 1, 1, "The largest nature reserve in Lebanon — famous for cedar trees, mountain landscapes, and long scenic trails.", "/images/activities/hiking/chouf.jpg", "Mount Lebanon", "Chouf hiking Trail", 0m },
                    { 35, 5, 1, "High-quality courts, popular among regular players.", "/images/activities/padel/the padelist.jpg", "zalka beirut Lebanon", "The padelist", 0m },
                    { 36, 5, 1, " Trendy location near the sea — great atmosphere and central location.", "/images/activities/padel/the padel club.jpg", "Beirut Waterfront (BIEL)", "The Padel Club", 0m },
                    { 37, 5, 1, "Premium vibe with padel + wellness concept — very modern place.", "/images/activities/padel/ClubHouse.jpg", "Dora", "Club House", 0m },
                    { 38, 5, 1, "Nice mountain area feel — good if you want to play outside Beirut.", "/images/activities/padel/PadelTown.jpg", "Ain Anoub (Mount Lebanon)", "Padel town", 0m },
                    { 39, 5, 1, " Beautiful seaside setting — very cool summer vibe.", "/images/activities/padel/padelByTheSea.jpg", "Halat (Jbeil coast)", "Padel by The Sea", 0m },
                    { 40, 5, 1, "One of the most popular padel spots with modern courts and active community.", "/images/activities/padel/padelHouse.jpg", "Jisr El Bacha – Metn", "Padel House", 0m },
                    { 41, 6, 1, "Tennis courts inside the luxury Mövenpick resort, offering a premium sports experience near the sea with professional facilities and a relaxing atmosphere.", "/images/activities/padel/Movenpick_tennis.jpg", "Raouché, Beirut", "Mövenpick Tennis Courts", 0m },
                    { 42, 6, 1, "A well-known sports complex featuring quality tennis courts, often used for training, tournaments, and recreational play in a calm mountain setting.", "/images/activities/padel/Mt_tennis.jpg", "Ain Saadeh – Metn", "Mont La Salle (Mt. Tennis)", 0m },
                    { 43, 6, 1, "Modern outdoor tennis courts designed for both practice and competitive play, offering a sporty atmosphere and coaching possibilities.", "/images/activities/padel/tennisClub.jpg", "Mount Lebanon", "Tennis Club Lebanon (Private Tennis Club)", 0m },
                    { 44, 4, 1, "The largest football stadium in Lebanon, hosting major national matches, tournaments, and international events.", "/images/activities/football/camilleChamoun-stad.jpg", "Beirut", "Camille Chamoun Sports City Stadium", 0m },
                    { 45, 4, 1, "A major football stadium in northern Lebanon used for league matches and local sports events.", "/images/activities/football/tripoliStadium.jpg", "Tripoli, North Lebanon", "Tripoli Municipal Stadium", 0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Slug",
                value: "Football");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Slug",
                value: "Padel");

            migrationBuilder.UpdateData(
                table: "ActivityTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Slug",
                value: "Teniis");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/images/activities/swimming/sporting.jpg", "Sporting Beach" });

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 20,
                column: "ImageUrl",
                value: "/images/activities/skiing/mzaar.jpg");

            migrationBuilder.UpdateData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 21,
                column: "ImageUrl",
                value: "/images/activities/hiking/chouwen.jpg");
        }
    }
}
