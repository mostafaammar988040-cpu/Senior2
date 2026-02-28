public class TripPlan
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; }

    public string Title { get; set; }

    public string ItineraryJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}