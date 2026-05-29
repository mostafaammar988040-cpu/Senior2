namespace Senior2.Api.DTOs.Advertisements
{
    using Senior2.Api.Models;
    // Application/DTOs/Advertisements/UpdateAdvertisementDto.cs
    using System.ComponentModel.DataAnnotations;


    [DateRange(nameof(StartDateUtc), nameof(EndDateUtc))]
    public sealed class UpdateAdvertisementDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTimeOffset StartDateUtc { get; set; }

        [Required]
        public DateTimeOffset EndDateUtc { get; set; }

        [Range(0, 1000)]
        public int Priority { get; set; }

        [StringLength(300)]
        public string? AdminNote { get; set; }
    }

}
