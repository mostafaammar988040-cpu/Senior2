using Senior2.Api.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<ActivityType> ActivityTypes { get; set; } = new List<ActivityType>();

    public ICollection<Place> Places { get; set; } = new List<Place>();
}
