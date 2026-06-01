using System;

namespace Senior2.Api.Models
{
    public class Suggestion
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Type { get; set; } = "";
        // place | feature | bug | general

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string? Location { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Users? User { get; set; }
    }
}