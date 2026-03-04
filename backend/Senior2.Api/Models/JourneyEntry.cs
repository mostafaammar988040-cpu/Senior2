using System.ComponentModel.DataAnnotations;

namespace Senior2.Api.Models
{
    public class JourneyEntry
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        // NEW 🔥
        public string? MediaUrl { get; set; }

        public string? MediaType { get; set; } // image / video

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}