namespace Senior2.Api.DTOs.Advertisements
{
    using Senior2.Api.Models;
    // Application/DTOs/Advertisements/CreateAdvertisementDto.cs
    using System.ComponentModel.DataAnnotations;


    [DateRange(nameof(StartDateUtc), nameof(EndDateUtc))]
    public sealed class CreateAdvertisementDto
    {
        [Required]
        public int PlaceId { get; set; }

        [Required]
        public DateTimeOffset StartDateUtc { get; set; }

        [Required]
        public DateTimeOffset EndDateUtc { get; set; }

        [Range(0, 1000)]
        public int Priority { get; set; } = 0;

        [StringLength(300)]
        public string? AdminNote { get; set; }
    }

}
