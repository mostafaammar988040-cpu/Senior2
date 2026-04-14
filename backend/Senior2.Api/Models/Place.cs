namespace Senior2.Api.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int? ActivityTypeId { get; set; }
        public ActivityType? ActivityType { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

    }
}
