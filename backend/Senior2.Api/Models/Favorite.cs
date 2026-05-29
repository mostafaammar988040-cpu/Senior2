using Senior2.Api.Models;

public class Favorite
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; }

    public int PlaceId { get; set; }
    public Place Place { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}