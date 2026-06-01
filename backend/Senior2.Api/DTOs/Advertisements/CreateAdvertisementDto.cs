namespace Senior2.Api.DTOs.Advertisements
{
    public class CreateAdvertisementDto
    {
        public int? PlaceId { get; set; }
        public DateTimeOffset StartDateUtc { get; set; }

        public DateTimeOffset EndDateUtc { get; set; }

        public int Priority { get; set; }

        public string? AdminNote { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}