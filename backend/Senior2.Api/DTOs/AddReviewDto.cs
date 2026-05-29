namespace Senior2.Api.DTOs
{
    public class AddReviewDto
    {
        public int PlaceId { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}