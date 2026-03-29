using Microsoft.EntityFrameworkCore;
using Senior2.Api.Models;

namespace Senior2.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ===============================
        // EXISTING TABLES
        // ===============================
        public DbSet<Category> Categories { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; } = null!;
        public DbSet<Place> Places { get; set; } = null!;
        public DbSet<Users> Users { get; set; }

        // ===============================
        // PROFILE SYSTEM (NEW)
        // ===============================
        public DbSet<UserPreference> UserPreferences { get; set; }

        public DbSet<TripPlan> TripPlans { get; set; }
        public DbSet<TripPlanPlace> TripPlanPlaces { get; set; }

        public DbSet<JourneyEntry> JourneyEntries { get; set; }

        public DbSet<Traveler> Travelers { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        // 🚨 THESE WERE MISSING
        public DbSet<SmartItineraryRequest> SmartItineraryRequest { get; set; }

        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<PlaceReview> PlaceReviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public object PlaceReview { get; internal set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // (YOUR EXISTING SEED DATA — KEEP EVERYTHING BELOW)

            // ===============================
            // 🟢 CATEGORIES
            // ===============================
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Activities", Slug = "activities", ImageUrl = "/images/categories/activities.jpg" },
                new Category { Id = 2, Name = "Guesthouses", Slug = "guesthouses", ImageUrl = "/images/categories/guesthouses.jpg" },
                new Category { Id = 3, Name = "Hotels", Slug = "hotels", ImageUrl = "/images/categories/hotels.jpg" },
                new Category { Id = 4, Name = "Restaurants", Slug = "restaurants", ImageUrl = "/images/categories/restaurants.jpg" }
            );

            // ===============================
            // 🟢 ACTIVITY TYPES
            // ===============================
            modelBuilder.Entity<ActivityType>().HasData(
                new ActivityType
                {
                    Id = 1,
                    Name = "Hiking",
                    Slug = "hiking",
                    ImageUrl = "/images/activities/hiking/chouwen_hike.jpg",
                    CategoryId = 1
                },
                new ActivityType
                {
                    Id = 2,
                    Name = "Swimming",
                    Slug = "swimming",
                    ImageUrl = "/images/activities/swimming/swimming.jpg",
                    CategoryId = 1
                },
                new ActivityType
                {
                    Id = 3,
                    Name = "Skiing",
                    Slug = "skiing",
                    ImageUrl = "/images/activities/skiing/skiing.jpg",
                    CategoryId = 1
                },
                  new ActivityType
                  {
                      Id = 4,
                      Name = "Football",
                      Slug = "football",
                      ImageUrl = "/images/activities/football/football.jpg",
                      CategoryId = 1
                  },
                   new ActivityType
                   {
                       Id = 5,
                       Name = "Padel",
                       Slug = "padel",
                       ImageUrl = "/images/activities/padel/padel.jpg",
                       CategoryId = 1

                   },
                    new ActivityType
                    {
                        Id = 6,
                        Name = "Tennis",
                        Slug = "tennis",
                        ImageUrl = "/images/activities/tennis/tennis.jpg",
                        CategoryId = 1
                    }
            );

            // ===============================
            // 🟢 PLACES
            // ===============================
            modelBuilder.Entity<Place>().HasData(

                // ===== 🏡 GUESTHOUSES (6) =====
                new Place
                {
                    Id = 1,
                    Name = "Beit Trad Guesthouse",
                    Description = "Traditional Lebanese mountain guesthouse.",
                    Location = "Broummana, Lebanon",
                    Price = 80,
                    ImageUrl = "/images/guesthouses/beit-trad/beitTrad_livingRoom.jpg",
                    CategoryId = 2
                },
                new Place
                {
                    Id = 2,
                    Name = "Charme Guesthouse",
                    Description = "Charming stay surrounded by nature.",
                    Location = "Cedars, Lebanon",
                    Price = 95,
                    ImageUrl = "/images/guesthouses/charme guesthouse/charme_overview.jpg",
                    CategoryId = 2
                },
                new Place
                {
                    Id = 3,
                    Name = "Beit Faris",
                    Description = "A charming guesthouse in Byblos.",
                    Location = "Byblos, Lebanon",
                    Price = 95,
                    ImageUrl = "/images/guesthouses/Beit Faris.jpg",
                    CategoryId = 2
                },
                new Place
                {
                    Id = 4,
                    Name = "Beit Toureef",
                    Description = "Beautiful heritage guesthouse.",
                    Location = "Gemmayzeh, Lebanon",
                    Price = 95,
                    ImageUrl = "/images/guesthouses/beitToureef/beitToureef_Overview.jpg",
                    CategoryId = 2
                },
                new Place
                {
                    Id = 5,
                    Name = "Beit Jeddé",
                    Description = "Warm village guesthouse in Mtein.",
                    Location = "Mtein, Lebanon",
                    Price = 95,
                    ImageUrl = "/images/guesthouses/beit-jedde/beitJeddi_Overview.jpg",
                    CategoryId = 2
                },
                new Place
                {
                    Id = 6,
                    Name = "Beit El Berbara",
                    Description = "Cozy boutique guesthouse by the sea.",
                    Location = "Barbara, Lebanon",
                    Price = 95,
                    ImageUrl = "/images/guesthouses/beit-elBarbara/beit-elBarbara.jpg",
                    CategoryId = 2
                },

                // ===== 🏨 HOTELS (6) =====
                new Place { Id = 7, Name = "Phoenicia Hotel", Description = "Luxury 5-star hotel.", Location = "Beirut", Price = 250, ImageUrl = "/images/hotels/phoenicia.jpg", CategoryId = 3 },
                new Place { Id = 8, Name = "Radisson Blu Hotel", Description = "Modern business hotel.", Location = "Verdun Beirut", Price = 250, ImageUrl = "/images/hotels/radisson.jpg", CategoryId = 3 },
                new Place { Id = 9, Name = "The Smallville Hotel", Description = "Trendy boutique hotel.", Location = "Badaro", Price = 250, ImageUrl = "/images/hotels/The Smallville Hotel.jpg", CategoryId = 3 },
                new Place { Id = 10, Name = "Kempinski Summerland", Description = "Luxury seaside resort.", Location = "Jnah Beirut", Price = 250, ImageUrl = "/images/hotels/Kempinski Summerland Hotel & Resort Beirut.jpg", CategoryId = 3 },
                new Place { Id = 11, Name = "Four Seasons Hotel", Description = "High-end luxury hotel.", Location = "Downtown Beirut", Price = 250, ImageUrl = "/images/hotels/Four Seasons Hotel Beirut.jpg", CategoryId = 3 },
                new Place { Id = 12, Name = "Le Gabriel Hotel", Description = "Elegant boutique hotel.", Location = "Achrafieh", Price = 250, ImageUrl = "/images/hotels/Le Gabriel.jpg", CategoryId = 3 },

                // ===== 🍽 RESTAURANTS (6) =====
                new Place { Id = 13, Name = "Em Sherif", Description = "Authentic Lebanese fine dining.", Location = "Beirut", Price = 50, ImageUrl = "/images/restaurants/emsherif.jpg", CategoryId = 4 },
                new Place { Id = 14, Name = "Bebabel", Description = "Modern Lebanese restaurant.", Location = "Beirut", Price = 50, ImageUrl = "/images/restaurants/Bebabel.jpg", CategoryId = 4 },
                new Place { Id = 15, Name = "Babel Bay", Description = "Upscale seafood dining.", Location = "Dbayeh", Price = 50, ImageUrl = "/images/restaurants/Babel.jpg", CategoryId = 4 },
                new Place { Id = 16, Name = "Al Beiruti", Description = "Traditional mezze and grills.", Location = "Beirut", Price = 50, ImageUrl = "/images/restaurants/Albeiruti.jpg", CategoryId = 4 },
                new Place { Id = 17, Name = "Liza", Description = "Elegant Lebanese-Mediterranean.", Location = "Achrafieh", Price = 50, ImageUrl = "/images/restaurants/Liza.jpg", CategoryId = 4 },
                new Place { Id = 18, Name = "Kampai", Description = "Japanese sushi & fusion.", Location = "Beirut", Price = 50, ImageUrl = "/images/restaurants/Kampai.jpg", CategoryId = 4 },

// ===== 🏄 ACTIVITIES =====

new Place
{
    Id = 19,
    Name = "Movenpick Beach",
    Description = "Popular swimming destination in Beirut.",
    Location = "Beirut",
    Price = 25,
    ImageUrl = "/images/activities/swimming/movenpick.jpg",
    CategoryId = 1,
    ActivityTypeId = 2 // Swimming
},
new Place
{
    Id = 22,
    Name = "Sporting Beach",
    Description = "Popular swimming destination in Beirut.",
    Location = "Beirut",
    Price = 25,
    ImageUrl = "/images/activities/swimming/sporting.jpg",
    CategoryId = 1,
    ActivityTypeId = 2 // Swimming
},
new Place
{
    Id = 23,
    Name = "Blubay Beach",
    Description = "Popular swimming destination in batroun.",
    Location = "batroun",
    Price = 25,
    ImageUrl = "/images/activities/swimming/blubay.jpg",
    CategoryId = 1,
    ActivityTypeId = 2 // Swimming
},
new Place
{
    Id = 24,
    Name = "Tyree Beach",
    Description = "Popular swimming destination in tyree.",
    Location = "South Lebanon (Sour)",
    Price = 25,
    ImageUrl = "/images/activities/swimming/tyree.jpg",
    CategoryId = 1,
    ActivityTypeId = 2 // Swimming
},
new Place
{
    Id = 25,
    Name = "Tahet el rich Beach",
    Description = "Popular swimming destination in anfeh.",
    Location = "Anfeh (North Lebanon)",
    Price = 25,
    ImageUrl = "/images/activities/swimming/tahetelrich.jpg",
    CategoryId = 1,
    ActivityTypeId = 2 // Swimming
},
new Place
{
    Id = 26,
    Name = "Lazy B  Beach",
    Description = "Popular swimming destination in jiyeh.",
    Location = "Jiyeh, South of Beirut (Mount Lebanon)",
    Price = 25,
    ImageUrl = "/images/activities/swimming/lazyb.jpg",
    CategoryId = 1,
    ActivityTypeId = 2 // Swimming
},
new Place
{
    Id = 20,
    Name = "Mzaar Ski Resort",
    Description = "Best skiing destination in Lebanon.",
    Location = "Kfardebian",
    Price = 60,
    ImageUrl = "/images/activities/skiing/mzaarSkiResort.jpg",
    CategoryId = 1,
    ActivityTypeId = 3 // Skiing
},
new Place
{
    Id = 27,
    Name = "Cedars Ski Resort",
    Description = "Best skiing destination in Lebanon.",
    Location = "arz",
    Price = 60,
    ImageUrl = "/images/activities/skiing/cedarsSkiResort.jpg",
    CategoryId = 1,
    ActivityTypeId = 3 // Skiing
},
new Place
{
    Id = 28,
    Name = "Laqlouq Ski Resort",
    Description = "Best skiing destination in Lebanon.",
    Location = "laqlouq",
    Price = 60,
    ImageUrl = "/images/activities/skiing/laqlouqSkiResort.jpg",
    CategoryId = 1,
    ActivityTypeId = 3 // Skiing
},
new Place
{
    Id = 29,
    Name = "Zaarour Ski Resort",
    Description = "Best skiing destination in Lebanon.",
    Location = "zaarour",
    Price = 60,
    ImageUrl = "/images/activities/skiing/zaarour.jpg",
    CategoryId = 1,
    ActivityTypeId = 3 // Skiing
},
new Place
{
    Id = 21,
    Name = "Chouwen Hiking Trail",
    Description = "Beautiful hiking area with river pools.",
    Location = "Jbeil",
    Price = 0,
    ImageUrl = "/images/activities/hiking/chouwen_hike.jpg",
    CategoryId = 1,
    ActivityTypeId = 1 // Hiking
},
new Place
{
    Id = 30,
    Name = "Wadi Qadisha Hiking Trail",
    Description = "A UNESCO-listed valley known for dramatic cliffs, ancient monasteries, and peaceful nature trails — one of the most iconic hikes in Lebanon.",
    Location = "North Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/hiking/wadi-qadisha.jpg",
    CategoryId = 1,
    ActivityTypeId = 1 // Hiking
},
new Place
{
    Id = 31,
    Name = "Tannourine hiking Trail",
    Description = "A beautiful cedar forest with marked trails, fresh air, and panoramic mountain views — perfect for nature lovers.",
    Location = "North Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/hiking/tannourine.jpg",
    CategoryId = 1,
    ActivityTypeId = 1 // Hiking
},
new Place
{
    Id = 32,
    Name = "Balou balaa hiking Trail",
    Description = "A spectacular natural sinkhole with waterfalls and bridges — short hike but incredible views, especially in spring.",
    Location = "North Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/hiking/balou3.jpg",
    CategoryId = 1,
    ActivityTypeId = 1 // Hiking
},
new Place
{
    Id = 33,
    Name = "Ehden  hiking Trail",
    Description = "A large protected area with rich biodiversity, cool weather, and multiple hiking trails ranging from easy to advanced.",
    Location = "North Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/hiking/ehden.jpg",
    CategoryId = 1,
    ActivityTypeId = 1 // Hiking
},
new Place
{
    Id = 34,
    Name = "Chouf hiking Trail",
    Description = "The largest nature reserve in Lebanon — famous for cedar trees, mountain landscapes, and long scenic trails.",
    Location = "Mount Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/hiking/chouf.jpg",
    CategoryId = 1,
    ActivityTypeId = 1 // Hiking
},
new Place
{
    Id = 35,
    Name = "The padelist",
    Description = "High-quality courts, popular among regular players.",
    Location = "zalka beirut Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/padel/the padelist.jpg",
    CategoryId = 1,
    ActivityTypeId = 5 // Hiking
},
new Place
{
    Id = 36,
    Name = "The Padel Club",
    Description = " Trendy location near the sea — great atmosphere and central location.",
    Location = "Beirut Waterfront (BIEL)",
    Price = 0,
    ImageUrl = "/images/activities/padel/the padel club.jpg",
    CategoryId = 1,
    ActivityTypeId = 5 // Hiking
},
new Place
{
    Id = 37,
    Name = "Club House",
    Description = "Premium vibe with padel + wellness concept — very modern place.",
    Location = "Dora",
    Price = 0,
    ImageUrl = "/images/activities/padel/ClubHouse.jpg",
    CategoryId = 1,
    ActivityTypeId = 5 // Hiking
},
new Place
{
    Id = 38,
    Name = "Padel town",
    Description = "Nice mountain area feel — good if you want to play outside Beirut.",
    Location = "Ain Anoub (Mount Lebanon)",
    Price = 0,
    ImageUrl = "/images/activities/padel/PadelTown.jpg",
    CategoryId = 1,
    ActivityTypeId = 5 // Hiking
},
new Place
{
    Id = 39,
    Name = "Padel by The Sea",
    Description = " Beautiful seaside setting — very cool summer vibe.",
    Location = "Halat (Jbeil coast)",
    Price = 0,
    ImageUrl = "/images/activities/padel/padelByTheSea.jpg",
    CategoryId = 1,
    ActivityTypeId = 5 // tennis
},
new Place
{
    Id = 40,
    Name = "Padel House",
    Description = "One of the most popular padel spots with modern courts and active community.",
    Location = "Jisr El Bacha – Metn",
    Price = 0,
    ImageUrl = "/images/activities/padel/padelHouse.jpg",
    CategoryId = 1,
    ActivityTypeId = 5 // tennis
},
new Place
{
    Id = 41,
    Name = "Mövenpick Tennis Courts",
    Description = "Tennis courts inside the luxury Mövenpick resort, offering a premium sports experience near the sea with professional facilities and a relaxing atmosphere.",
    Location = "Raouché, Beirut",
    Price = 0,
    ImageUrl = "/images/activities/tennis/Movenpick_tennis.jpg",
    CategoryId = 1,
    ActivityTypeId = 6 // tennis
},
new Place
{
    Id = 42,
    Name = "Mont La Salle (Mt. Tennis)",
    Description = "A well-known sports complex featuring quality tennis courts, often used for training, tournaments, and recreational play in a calm mountain setting.",
    Location = "Ain Saadeh – Metn",
    Price = 0,
    ImageUrl = "/images/activities/tennis/Mt_tennis.jpg",
    CategoryId = 1,
    ActivityTypeId = 6 // tennis
},
new Place
{
    Id = 43,
    Name = "Tennis Club Lebanon (Private Tennis Club)",
    Description = "Modern outdoor tennis courts designed for both practice and competitive play, offering a sporty atmosphere and coaching possibilities.",
    Location = "Mount Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/tennis/tennisClub.jpg",
    CategoryId = 1,
    ActivityTypeId = 6 // tennis
},
new Place
{
    Id = 44,
    Name = "Camille Chamoun Sports City Stadium",
    Description = "The largest football stadium in Lebanon, hosting major national matches, tournaments, and international events.",
    Location = "Beirut",
    Price = 0,
    ImageUrl = "/images/activities/football/camilleChamoun-stad.jpg",
    CategoryId = 1,
    ActivityTypeId = 4 // football
},

new Place
{
    Id = 45,
    Name = "Tripoli Municipal Stadium",
    Description = "A major football stadium in northern Lebanon used for league matches and local sports events.",
    Location = "Tripoli, North Lebanon",
    Price = 0,
    ImageUrl = "/images/activities/football/triploiStadium.jpg",
    CategoryId = 1,
    ActivityTypeId = 4 // football
}

            );
        }
    }
}