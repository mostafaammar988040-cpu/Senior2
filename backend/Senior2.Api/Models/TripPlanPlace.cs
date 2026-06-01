namespace Senior2.Api.Models
{
    public class TripPlanPlace
    {
        public int Id { get; set; }

        public int TripPlanId { get; set; }
        public TripPlan TripPlan { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}