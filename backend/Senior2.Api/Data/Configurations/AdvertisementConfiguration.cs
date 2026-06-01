using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senior2.Api.Models;

namespace Senior2.Api.Data.Configurations
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Priority)
                   .HasDefaultValue(0);

            builder.Property(a => a.Status)
                   .IsRequired();

            builder.Property(a => a.CreatedAtUtc)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.RowVersion)
                   .IsRowVersion();

            // Relationship
            builder.HasOne(a => a.Place)
                   .WithMany(p => p.Advertisements)
                   .HasForeignKey(a => a.PlaceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}