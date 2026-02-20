namespace Senior2.Api.Models
{
    public class ActivityType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // URL friendly name (hiking, swimming, skiing)
        public string Slug { get; set; } = string.Empty;

        // Card image
        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}