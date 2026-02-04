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

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ActivityType> ActivityTypes { get; set; } = null!;
        public DbSet<Place> Places { get; set; } = null!;
        public DbSet<Users> Users { get; set; }

    }
}
