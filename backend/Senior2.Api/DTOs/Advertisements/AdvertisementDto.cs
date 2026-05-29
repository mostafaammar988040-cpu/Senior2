namespace Senior2.Api.DTOs.Advertisements
{
    // Application/DTOs/Advertisements/AdvertisementDto.cs
    using Senior2.Api.Models;


    public sealed class AdvertisementDto
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string PlaceName { get; set; } = string.Empty;
        public DateTimeOffset StartDateUtc { get; set; }
        public DateTimeOffset EndDateUtc { get; set; }
        public int Priority { get; set; }
        public AdvertisementStatus Status { get; set; }
        public string? AdminNote { get; set; }
        public bool IsActive { get; set; }
    }

}
