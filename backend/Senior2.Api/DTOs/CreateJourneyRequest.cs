using Microsoft.AspNetCore.Http;

namespace Senior2.Api.DTOS
{
    public class CreateJourneyRequest
    {
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public IFormFile? Media { get; set; }

        // NEW: whether this journey should be shared
        public bool IsShared { get; set; } = false;
    }
}