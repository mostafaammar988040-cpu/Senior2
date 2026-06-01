namespace Senior2.Api.Models
{
    public class PlaceReview
    {
        public int Id { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; } = null!;

        public int UserId { get; set; }
        public Users User { get; set; } = null!;

        public int Rating { get; set; } // 1-5

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}