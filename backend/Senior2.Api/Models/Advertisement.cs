namespace Senior2.Api.Models
{
    // Domain/Entities/Advertisement.cs
    using System.ComponentModel.DataAnnotations;
   

    [DateRange(nameof(StartDateUtc), nameof(EndDateUtc))]
    public class Advertisement
    {
        public int Id { get; set; }

        [Required]
        public int PlaceId { get; set; }

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

        public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAtUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public Place Place { get; set; } = null!;
    }

}
