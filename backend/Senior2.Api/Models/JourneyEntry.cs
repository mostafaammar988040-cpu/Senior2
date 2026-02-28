public class JourneyEntry
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}