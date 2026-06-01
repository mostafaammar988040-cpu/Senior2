namespace Senior2.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    [DateRange(nameof(StartDateUtc), nameof(EndDateUtc))]
    public class Advertisement
    {
        public int Id { get; set; }

        // Place is now optional because the ad can be created using only an uploaded image
        public int? PlaceId { get; set; }

        [Required]
        public DateTimeOffset StartDateUtc { get; set; }

        [Required]
        public DateTimeOffset EndDateUtc { get; set; }

        [Range(0, 1000)]
        public int Priority { get; set; } = 0;

        [Required]
        public AdvertisementStatus Status { get; set; } = AdvertisementStatus.Pending;

        [StringLength(300)]
        public string? AdminNote { get; set; }

        public string? ImageUrl { get; set; }

        public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAtUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Nullable because custom ads may not be linked to a place
        public Place? Place { get; set; }
    }
}