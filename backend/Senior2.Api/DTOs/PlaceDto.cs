namespace Senior2.Api.DTOs
{
    public class PlaceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public int? ActivityTypeId { get; set; }
    }
}
